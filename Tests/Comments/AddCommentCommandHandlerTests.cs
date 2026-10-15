using Application.Common.Errors;
using Application.Features.Comments.Commands.CreateComment;
using Application.Features.Comments.Specifications;
using Application.Interfaces.Common;
using Application.Interfaces.Persistence;
using Application.Specifications;
using Domain.Entities;
using Domain.Enums;
using NSubstitute;
using NSubstitute.Core;
using OneOf;
using Task = Domain.Entities.Task;

namespace Application.Test.Comments
{
    public class AddCommentCommandHandlerTests
    {
        [Fact]
        public async System.Threading.Tasks.Task AddComment_NonexistentTask_ReturnsNotFoundError()
        {

            // Arrange
            var currentUserService = Substitute.For<ICurrentUserService>();
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var taskRepository = Substitute.For<IGenericRepository<Task>>();

            unitOfWork.Repository<Task>().Returns(taskRepository);


            currentUserService.GetCurrentUserId().Returns(1);
            currentUserService.GetCurrentUserRoles().Returns(["Employee"]);

            taskRepository.Get(Arg.Any<GetTaskByPublicIdWithComments>(), Arg.Any<CancellationToken>()).Returns(System.Threading.Tasks.Task.FromResult<Task?>(null));


            var command = new CreateCommentCommand(Guid.NewGuid(),"Content");
            var handler = new CreateCommentCommandHandler(unitOfWork,currentUserService);
            var requestHandler = (MediatR.IRequestHandler<CreateCommentCommand, OneOf<Guid, UnauthorizedError, NotFoundError, ForbiddenError>>)handler;

            // act
            var result = await requestHandler.Handle(command, CancellationToken.None);


            // Assert
            Assert.True(result.IsT2);
            await unitOfWork
                .DidNotReceive()
                .Complete(Arg.Any<CancellationToken>());


        }

        [Fact]
        public async System.Threading.Tasks.Task AddComment_ManagerNotOwningProject_ReturnsForbiddenError()
        {


            // Arrange
            var now = DateTimeOffset.UtcNow;
            var managerId = 1;
            var userId = 3;
            var taskId = Guid.NewGuid();
            var project = new Project("Project 1", "Description", now, now.AddDays(10), managerId);
            var task = project.AddTask("Task 1", "Description", now, now.AddDays(5), taskId);
            var commentsNumber = task.Comments.Count;
            var currentUserService = Substitute.For<ICurrentUserService>();
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var taskRepository = Substitute.For<IGenericRepository<Task>>();
            var projectRepository = Substitute.For<IProjectRepository>();

            unitOfWork.Repository<Task>().Returns(taskRepository);
            unitOfWork.ProjectRepository.Returns(projectRepository);

            currentUserService.GetCurrentUserId().Returns(userId);
            currentUserService.GetCurrentUserRoles().Returns([UserRoles.Manager.ToString()]);


            taskRepository.Get(Arg.Any<GetTaskByPublicIdWithComments>(), Arg.Any<CancellationToken>()).Returns(task);

            projectRepository.GetManagerId(task.ProjectId, Arg.Any<CancellationToken>()).Returns(managerId);


            var command = new CreateCommentCommand(taskId, "Content");
            var handler = new CreateCommentCommandHandler(unitOfWork, currentUserService);
            var requestHandler = (MediatR.IRequestHandler<CreateCommentCommand, OneOf<Guid, UnauthorizedError, NotFoundError, ForbiddenError>>)handler;

            // act
            var result = await requestHandler.Handle(command, CancellationToken.None);


            // Assert
            Assert.True(result.IsT3);
            Assert.Equal(commentsNumber,task.Comments.Count);
            await unitOfWork
                .DidNotReceive()
                .Complete(Arg.Any<CancellationToken>());

        }

        [Fact]
        public async System.Threading.Tasks.Task AddComment_EmployeeNotAssignedToTask_ReturnsForbiddenError()
        {


            // Arrange
            var now = DateTimeOffset.UtcNow;
            var managerId = 1;
            var userId = 3;
            var taskId = Guid.NewGuid();
            var project = new Project("Project 1", "Description", now, now.AddDays(10), managerId);
            var task = project.AddTask("Task 1", "Description", now, now.AddDays(5), taskId);
            var commentsNumber = task.Comments.Count;
            var currentUserService = Substitute.For<ICurrentUserService>();
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var taskRepository = Substitute.For<IGenericRepository<Task>>();
            var projectRepository = Substitute.For<IProjectRepository>();
            var taskAssignmentRepository = Substitute.For<IGenericRepository<TaskAssignment>>();
            unitOfWork.Repository<Task>().Returns(taskRepository);
            unitOfWork.Repository<TaskAssignment>().Returns(taskAssignmentRepository);
            unitOfWork.ProjectRepository.Returns(projectRepository);

            currentUserService.GetCurrentUserId().Returns(userId);
            currentUserService.GetCurrentUserRoles().Returns([UserRoles.Employee.ToString()]);


            taskRepository.Get(Arg.Any<GetTaskByPublicIdWithComments>(), Arg.Any<CancellationToken>()).Returns(task);
            taskAssignmentRepository.Exists(Arg.Any<GetRecordsByCriteriaSpecification<TaskAssignment>>(), Arg.Any<CancellationToken>()).Returns(false);
            projectRepository.GetManagerId(task.ProjectId, Arg.Any<CancellationToken>()).Returns(managerId);


            var command = new CreateCommentCommand(taskId, "Content");
            var handler = new CreateCommentCommandHandler(unitOfWork, currentUserService);
            var requestHandler = (MediatR.IRequestHandler<CreateCommentCommand, OneOf<Guid, UnauthorizedError, NotFoundError, ForbiddenError>>)handler;

            // act
            var result = await requestHandler.Handle(command, CancellationToken.None);


            // Assert
            Assert.True(result.IsT3);
            Assert.Equal(commentsNumber, task.Comments.Count);
            await unitOfWork
                .DidNotReceive()
                .Complete(Arg.Any<CancellationToken>());

        }
    }
}
