using Domain.Exceptions;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Domain.Test.Tasks
{
    public class TaskStatusTransitionTest
    {
        [Fact]
        public void CompleteTask_InProgressTask_ChangesStatusToCompleted()
        {
            //Arrange

            var project = new Domain.Entities.Project("Project 1", "Description", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(10), 1);
            var taskId = Guid.NewGuid();
            project.AddTask("Task 1", "Description", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(5), taskId, TaskStatus.InProgress);
            var task = project.Tasks.First(t => t.PublicId == taskId);
            // Act
            project.CompleteTask(task.PublicId);
            //Assert
            Assert.Equal(TaskStatus.Completed, task.TaskStatus);
        }

        [Fact]
        public void CompleteTask_PendingTask_RejectsTransition()
        {
            //Arrange
            var project = new Domain.Entities.Project("Project 1", "Description", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(10), 1);
            project.AddTask("Task 1", "Description", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(5), null, TaskStatus.Cancelled);
            var task = project.Tasks.First();
            // Act
            var act = () => project.CompleteTask(task.PublicId);

            //Assert
            Assert.Throws<InvalidOperation>(act);
        }


    }
}
