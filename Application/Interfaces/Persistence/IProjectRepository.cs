using Domain.Entities.ProjectAggregate;
using Task = Domain.Entities.ProjectAggregate.Task;
namespace Application.Interfaces.Persistence
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
    }
}
