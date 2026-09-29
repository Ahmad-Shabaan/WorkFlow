using Application.Specifications;
using Domain.Entities;

namespace Application.Features.Projects.Specifications
{
    public class GetProjectByManagerIdSpecification(int managerId) : BaseSpecification<Project>(p => p.ManagerId == managerId)
    {
    }
}
