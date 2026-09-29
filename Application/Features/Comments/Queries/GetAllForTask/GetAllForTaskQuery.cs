using Application.Common.Errors;
using Application.Features.Comments.DTOs;
using MediatR;
using OneOf;
namespace Application.Features.Comments.Queries.GetAllForTask
{
    public sealed record GetAllForTaskQuery(Guid TaskId) : IRequest<OneOf<IReadOnlyList<CommentResponseDto>, NotFoundError>>
    {
    }
}
