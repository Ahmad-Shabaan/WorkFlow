using Application.Common.Errors;
using Application.Features.Projects.DTOs;
using MediatR;
using OneOf;
namespace Application.Features.Projects.Queries.GetProject
{
    public sealed record GetProjectQuery(Guid PublicId) : IRequest<OneOf<ProjectDto,NotFoundError>>
    {
    }
}
