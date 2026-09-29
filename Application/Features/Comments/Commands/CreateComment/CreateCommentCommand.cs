using Application.Common.Errors;
using MediatR;
using OneOf;
namespace Application.Features.Comments.Commands.CreateComment
{
    public sealed record CreateCommentCommand(Guid TaskId ,string Content) : IRequest<OneOf<Guid, UnauthorizedError,NotFoundError, ForbiddenError>>
    {
    }
}
