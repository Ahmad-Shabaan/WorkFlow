using Application.Common.Errors;
using Application.Errors;
using Application.Features.Comments.Specifications;
using Application.Interfaces.Common;
using Application.Interfaces.Persistence;
using MediatR;
using OneOf;
using OneOf.Types;
using Task = Domain.Entities.Task;
namespace Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) :
       IRequestHandler<UpdateCommentCommand, OneOf<Success, UnauthorizedError, NotFoundError, ForbiddenError>>
    {
        public async System.Threading.Tasks.Task<OneOf<Success, UnauthorizedError, NotFoundError, ForbiddenError>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.GetCurrentUserId();
            if (userId is null)
                return AuthenticationErrors.Unauthorized;
            var taskSpec = new GetTaskByPublicIdWithComments(request.TaskId);
            var task = await unitOfWork.Repository<Task>().Get(taskSpec, cancellationToken);
            if (task is null)
                return TaskErrors.NotFoundError;

            var isOwner = task.Comments.Any(c => c.AuthorId == userId && c.PublicId == request.CommentId);
            if (!isOwner)
                return AuthenticationErrors.Forbidden;

            task.UpdateComment(request.CommentId, request.Content);
            await unitOfWork.Complete(cancellationToken);
            return new Success();
        }
    }
}
