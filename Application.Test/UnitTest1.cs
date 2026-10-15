using Application.Common.Errors;
using Application.Features.Authentication.DTOs;
using Application.Features.Tasks.Commands.AssignToTask;
using Application.Interfaces.Common;
using Application.Interfaces.Persistence;
using Domain.Entities;
using OneOf;
using OneOf.Types;

namespace Application.Test
{
    public class UnitTest1
    {

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_NonexistentUser_IsRejected()
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

            var project = new Project(
                "Project 1",
                "Description",
                now,
                now.AddDays(10),
                managerId);

            var task = project.AddTask(
                "Task 1",
                "Description",
                now,
                now.AddDays(5),
                taskPublicId);

            var taskRepository = Substitute.For<ITaskRepository>();
            var projectRepository = Substitute.For<IGenericRepository<Project>>();
            var assignmentRepository =
                Substitute.For<IGenericRepository<TaskAssignment>>();

            currentUserService.GetCurrentUserId().Returns(managerId);

            unitOfWork.TaskRepository.Returns(taskRepository);


            taskRepository.GetByPublicId(taskPublicId, Arg.Any<CancellationToken>())
                .Returns(task);

            unitOfWork.ProjectRepository.Returns(projectRepository);

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

    }
}