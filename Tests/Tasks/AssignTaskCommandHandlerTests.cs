using Application.Common.Errors;
using Application.Features.Authentication.DTOs;
using Application.Features.Tasks.Commands.AssignToTask;
using Application.Interfaces.Common;
using Application.Interfaces.Persistence;
using Application.Specifications;
using Domain.Entities;
using Domain.Enums;
using NSubstitute;
using OneOf;
using OneOf.Types;
namespace Application.Test.Tasks
{
    public class AssignTaskCommandHandlerTests
    {

        [Fact]
        public async System.Threading.Tasks.Task AssignToTask_NonexistentUser_ReturnsNotFoundError()
        {
            // Arrange
            // 1. Current user is authenticated.
            // 2. Task exists.
            // 3. Project exists.
            // 4. Current user is the project manager.
            // 5. Target user does not exist.

            var unitOfWork = Substitute.For<IUnitOfWork>();
            var identityService = Substitute.For<IIdentityService>();
            var currentUserService = Substitute.For<ICurrentUserService>();
            const int managerId = 10;
            var taskPublicId = Guid.NewGuid();
            var targetUserId = Guid.NewGuid();
            var now = DateTimeOffset.UtcNow;

            var project = new Project("Project 1", "Description", now, now.AddDays(10), managerId);

            var task = project.AddTask("Task 1", "Description", now, now.AddDays(5), taskPublicId);

            var taskRepository = Substitute.For<ITaskRepository>();
            var projectRepository = Substitute.For<IGenericRepository<Project>>();
            var assignmentRepository =
                Substitute.For<IGenericRepository<TaskAssignment>>();

            currentUserService.GetCurrentUserId().Returns(managerId);

            unitOfWork.TaskRepository.Returns(taskRepository);


            taskRepository.GetByPublicId(taskPublicId, Arg.Any<CancellationToken>())
                .Returns(task);

            unitOfWork.Repository<Project>().Returns(projectRepository);

            projectRepository.Get(task.ProjectId, Arg.Any<CancellationToken>())
                 .Returns(project);


            // here we simulate that the target user does not exist in the identity service
            identityService.GetUser(targetUserId).Returns(System.Threading.Tasks.Task.FromResult<UserInfo?>(null));

            unitOfWork.Repository<TaskAssignment>().Returns(assignmentRepository);

            var command = new AssignToTaskCommand(taskPublicId, targetUserId);
            var handler = new AssignToTaskCommandHandler(unitOfWork, identityService, currentUserService);
            var requestHandler = (MediatR.IRequestHandler<AssignToTaskCommand, OneOf<Success, UnauthorizedError, NotFoundError, ForbiddenError, ConflictError>>)handler;
            // Act
            // Execute the handler.
            var result = await requestHandler.Handle(command, CancellationToken.None);

            // Assert
            // 1. The result represents NotFoundError.
            // 2. No TaskAssignment was added.
            // 3. UnitOfWork.Complete was not called.
            Assert.True(result.IsT2);

            assignmentRepository
                .DidNotReceive()
                .Add(Arg.Any<TaskAssignment>());

            await unitOfWork
                .DidNotReceive()
                .Complete(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async System.Threading.Tasks.Task AssignToTask_UserHasMaximumActiveTasks_ReturnsConflictError()
        {

            // Arrange
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var identityService = Substitute.For<IIdentityService>();
            var currentUserService = Substitute.For<ICurrentUserService>();

            var managerId = 10;
            var taskPublicId = Guid.NewGuid();
            var targetUserId = Guid.NewGuid();
            var now = DateTimeOffset.UtcNow;

            var project = new Project("Project 1", "Description", now, now.AddDays(10), managerId);
            var task = project.AddTask("Task 1", "Description", now, now.AddDays(5), taskPublicId);
            var userInfo = new UserInfo(1, targetUserId, "Ahmed", "ahmed@gmail.com", null, [UserRoles.Employee.ToString()]);
            var taskRepository = Substitute.For<ITaskRepository>();
            var projectRepository = Substitute.For<IGenericRepository<Project>>();
            var assignRepository = Substitute.For<IGenericRepository<TaskAssignment>>();
            unitOfWork.TaskRepository.Returns(taskRepository);
            unitOfWork.Repository<Project>().Returns(projectRepository);
            unitOfWork.Repository<TaskAssignment>().Returns(assignRepository);

            currentUserService.GetCurrentUserId().Returns(managerId);
            unitOfWork.Repository<Project>().Get(task.ProjectId, Arg.Any<CancellationToken>()).Returns(project);
            unitOfWork.TaskRepository.GetByPublicId(taskPublicId).Returns(task);
            identityService.GetUser(targetUserId).Returns(userInfo);

            unitOfWork.Repository<TaskAssignment>().Exists(Arg.Any<GetRecordsByCriteriaSpecification<TaskAssignment>>(), Arg.Any<CancellationToken>()).Returns(false);
            unitOfWork.Repository<TaskAssignment>().GetCount(Arg.Any<GetRecordsByCriteriaSpecification<TaskAssignment>>(), Arg.Any<CancellationToken>()).Returns(3);


            var command = new AssignToTaskCommand(taskPublicId, targetUserId);
            var handler = new AssignToTaskCommandHandler(unitOfWork, identityService, currentUserService);
            var requestHandler = (MediatR.IRequestHandler<AssignToTaskCommand, OneOf<Success, UnauthorizedError, NotFoundError, ForbiddenError, ConflictError>>)handler;
            // act
            var result = await requestHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsT4);


        }


    }
}
