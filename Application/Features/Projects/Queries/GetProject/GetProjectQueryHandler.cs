using Application.Common.Errors;
using Application.Errors;
using Application.Features.Projects.DTOs;
using Application.Features.Tasks.DTOs;
using Application.Interfaces.Persistence;
using AutoMapper;
using MediatR;
using OneOf;

namespace Application.Features.Projects.Queries.GetProject
{
    public class GetProjectQueryHandler(IIdentityService identityService, IProjectRepository projectRepository, IMapper mapper) : IRequestHandler<GetProjectQuery, OneOf<ProjectResponseDto, NotFoundError>>
    {

        public async Task<OneOf<ProjectResponseDto, NotFoundError>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetProjectByPublicId(request.PublicId, cancellationToken);
            if (project == null)
                return ProjectErrors.NotFoundError;
            var manager = await identityService.GetUserName(project.ManagerId);
            if (manager is null)
                return ProjectErrors.NotFoundError;
            return new ProjectResponseDto(manager.EmployeeId, manager.FullName, project.PublicId, project.ProjectName, project.ProjectDescription, project.StartDate, project.EndDate, project.ProjectStatus.ToString(),mapper.Map<IReadOnlyList<TaskDto>>(project.Tasks));
        }
    }
}
