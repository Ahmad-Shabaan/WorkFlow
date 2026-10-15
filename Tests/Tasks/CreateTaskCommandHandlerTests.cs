using Application.Common.Errors;
using Application.Features.Projects.Specifications;
using Application.Features.Tasks.Commands.CreateTask;
using Application.Interfaces.Persistence;
using Domain.Entities;
using NSubstitute;
using OneOf;
namespace Application.Test.Tasks
{
    public class CreateTaskCommandHandlerTests
    {
        [Fact]
        public async System.Threading.Tasks.Task CreateTask_NonexistentProject_ReturnsNotFoundError()
        {

            // Arrange
            var now = DateTimeOffset.UtcNow;
            var projectId = Guid.NewGuid();
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var projectRepository = Substitute.For<IGenericRepository<Project>>();
            unitOfWork.Repository<Project>().Returns(projectRepository);

            projectRepository.Get(Arg.Any<GetProjectByPublicIdWithTasks>(), Arg.Any<CancellationToken>()).Returns(System.Threading.Tasks.Task.FromResult<Project?>(null));

            var command = new CreateTaskCommand(projectId, "Task1", "Description", now, now.AddDays(10));
            var handler = new CreateTaskCommandHandler(unitOfWork);
            var requestHandler = (MediatR.IRequestHandler<CreateTaskCommand, OneOf<Guid, NotFoundError, ConflictError>>)handler;

            // act
            var result = await requestHandler.Handle(command, CancellationToken.None);


            // Assert
            Assert.True(result.IsT1);

            await unitOfWork
                .DidNotReceive()
                .Complete(Arg.Any<CancellationToken>());

        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_NonexistentProject_ReturnsConflictError()
        {

            // Arrange
            var now = DateTimeOffset.UtcNow;
            var project = new Project("Project 1", "Description", now, now.AddDays(10), 1);
            project.AddTask("Task1", "Description1", now, now.AddDays(5), Guid.NewGuid());
            var tasksNumber = project.Tasks.Count;

            var unitOfWork = Substitute.For<IUnitOfWork>();
            var projectRepository = Substitute.For<IGenericRepository<Project>>();
            unitOfWork.Repository<Project>().Returns(projectRepository);

            projectRepository.Get(Arg.Any<GetProjectByPublicIdWithTasks>(), Arg.Any<CancellationToken>()).Returns(project);

            var command = new CreateTaskCommand(project.PublicId, "Task1", "Description", now, now.AddDays(10));
            var handler = new CreateTaskCommandHandler(unitOfWork);
            var requestHandler = (MediatR.IRequestHandler<CreateTaskCommand, OneOf<Guid, NotFoundError, ConflictError>>)handler;

            // act
            var result = await requestHandler.Handle(command, CancellationToken.None);


            // Assert
            Assert.True(result.IsT2);
            Assert.Equal(tasksNumber, project.Tasks.Count);

            await unitOfWork.DidNotReceive().Complete(Arg.Any<CancellationToken>());

        }
    }
}
