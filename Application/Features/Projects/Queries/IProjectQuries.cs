using Domain.Entities.ProjectAggregate;

namespace Application.Features.Projects.Queries
{
    public interface IProjectQuries
    {
        Task<Project?> GetProjectByPublicId(Guid publicId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Project>> GetAllProjects(CancellationToken cancellationToken);

    }
}