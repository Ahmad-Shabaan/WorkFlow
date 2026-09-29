using Task = Domain.Entities.Task;

namespace Application.Interfaces.Persistence
{
    public interface ITaskRepository : IGenericRepository<Task>
    {
        Task<Task?> Get(Guid publicId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Task>> GetAllAsync(CancellationToken cancellationToken);
        Task<Task?> GetByPublicId(Guid publicId, CancellationToken cancellationToken = default);
        Task<Task?> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken);
    }
}
