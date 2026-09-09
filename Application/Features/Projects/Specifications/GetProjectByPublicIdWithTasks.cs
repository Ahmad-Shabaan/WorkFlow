using Application.Specifications;
using Domain.Entities.ProjectAggregate;
namespace Application.Features.Projects.Specifications
{
    public class GetProjectByPublicIdWithTasks : BaseSpecification<Project>
    {
        public GetProjectByPublicIdWithTasks(Guid publicId) : base(p => p.PublicId == publicId)
        {
            Includes.Add(project => project.Tasks);
        }
    }
}
