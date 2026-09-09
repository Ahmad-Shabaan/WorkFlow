using Application.Features.Projects.DTOs;
using MediatR;
namespace Application.Features.Projects.Queries.GetAllProjects
{
    public sealed record GetAllProjectsQuery : IRequest<IReadOnlyList<ProjectListDto>>
    {
    }
}
