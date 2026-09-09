using Task = Domain.Entities.ProjectAggregate.Task;

namespace Application.Features.Tasks.Queries
{
    public interface ITaskQuries
    {
        Task<IEnumerable<Task>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Task>> GetAllTasksInProjectAsync(Guid projectPublicId, CancellationToken cancellationToken);
        Task<Task?> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken);



    }
}