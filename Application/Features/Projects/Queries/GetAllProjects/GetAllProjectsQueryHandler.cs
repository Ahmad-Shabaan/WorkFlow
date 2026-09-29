using Application.Features.Projects.DTOs;
using Application.Interfaces.Persistence;
using MediatR;
namespace Application.Features.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQueryHandler(IProjectRepository projectRepository) : IRequestHandler<GetAllProjectsQuery, IReadOnlyList<ProjectListDto>>
    {
        public async Task<IReadOnlyList<ProjectListDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await projectRepository.GetAllProjects(cancellationToken);
            return projects.Select(p => new ProjectListDto(p.PublicId, p.ProjectName, p.ProjectDescription, p.StartDate, p.EndDate, p.ProjectStatus)).ToList();
        }
    }
}
