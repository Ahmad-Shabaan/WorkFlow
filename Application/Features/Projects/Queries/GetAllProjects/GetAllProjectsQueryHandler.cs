using Application.Features.Projects.DTOs;
using MediatR;
namespace Application.Features.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQueryHandler(IProjectQuries projectQuries) : IRequestHandler<GetAllProjectsQuery, IReadOnlyList<ProjectListDto>>
    {
        public async Task<IReadOnlyList<ProjectListDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await projectQuries.GetAllProjects(cancellationToken);
            return projects.Select(p => new ProjectListDto(p.PublicId, p.ProjectName, p.ProjectDescription, p.StartDate, p.EndDate, p.ProjectStatus)).ToList();
        }
    }
}
