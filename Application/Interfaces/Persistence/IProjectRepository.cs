using Domain.Entities;
using Task = Domain.Entities.Task;
namespace Application.Interfaces.Persistence
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<Project?> Get(Guid publicId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Project>> GetAllProjects(CancellationToken cancellationToken);
        Task<Project?> GetByPublicId(Guid publicId, CancellationToken cancellationToken = default);
        Task<int> GetManagerId(int projectId, CancellationToken cancellationToken);
        Task<Project?> GetProjectByPublicId(Guid publicId, CancellationToken cancellationToken);
    }
}
