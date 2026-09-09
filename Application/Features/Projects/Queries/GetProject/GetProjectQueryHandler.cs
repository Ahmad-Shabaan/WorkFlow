


using Application.Common.Errors;
using Application.Errors;
using Application.Features.Projects.DTOs;
using Application.Features.Tasks.DTOs;
using AutoMapper;
using Domain.Entities.ProjectAggregate;
using MediatR;
using OneOf;
using Task = Domain.Entities.ProjectAggregate.Task;

namespace Application.Features.Projects.Queries.GetProject
{
    public class GetProjectQueryHandler(IProjectQuries projectQuries, IMapper mapper) : IRequestHandler<GetProjectQuery, OneOf<ProjectDto, NotFoundError>>
    {

        public async Task<OneOf<ProjectDto, NotFoundError>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await projectQuries.GetProjectByPublicId(request.PublicId, cancellationToken);
            if (project == null)
                return ProjectErrors.NotFoundError;
            var dto = mapper.Map<ProjectDto>(project);
            return dto;
        }
    }
}
