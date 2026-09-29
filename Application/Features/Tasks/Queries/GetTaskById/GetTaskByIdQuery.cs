using Application.Common.Errors;
using Application.Features.Tasks.DTOs;
using MediatR;
using OneOf;
namespace Application.Features.Tasks.Queries.GetTaskById
{
    public  sealed record GetTaskByIdQuery(Guid TaskPublicId) : IRequest<OneOf<TaskResponseDto, NotFoundError>>
    {
    }
}
