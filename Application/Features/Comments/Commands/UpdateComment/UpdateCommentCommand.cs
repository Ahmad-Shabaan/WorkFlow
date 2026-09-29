
using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Features.Comments.Commands.UpdateComment
{
   public sealed record UpdateCommentCommand(Guid TaskId,Guid CommentId, string Content) : IRequest<OneOf<Success, UnauthorizedError, NotFoundError, ForbiddenError>>
    {
    }
}
