using Application.Common.Errors;
using Application.Errors;
using Application.Features.Comments.Specifications;
using Application.Features.Projects.Specifications;
using Application.Interfaces.Common;
using Application.Interfaces.Persistence;
using Application.Specifications;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using OneOf;
using Task = Domain.Entities.Task;
namespace Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) :
        IRequestHandler<CreateCommentCommand, OneOf<Guid, UnauthorizedError, NotFoundError, ForbiddenError>>
    {
        public async System.Threading.Tasks.Task<OneOf<Guid, UnauthorizedError, NotFoundError, ForbiddenError>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.GetCurrentUserId();
            var userRoles = currentUserService.GetCurrentUserRoles();

            if (userId is null || userRoles is null)
                return AuthenticationErrors.Unauthorized;

            var taskSpec = new GetTaskByPublicIdWithComments(request.TaskId);
            var task = await unitOfWork.Repository<Task>().Get(taskSpec, cancellationToken);
            if (task is null)
                return TaskErrors.NotFoundError;
            if (userRoles.Any(r => r == UserRoles.Manager.ToString()))
            {
                var managerId = await unitOfWork.ProjectRepository.GetManagerId(task.ProjectId, cancellationToken);
                if (userId != managerId)
                    return AuthenticationErrors.Forbidden;
            }
            else
            {
                var taskAssignmentSpec = new GetRecordsByCriteriaSpecification<TaskAssignment>(x => x.TaskId == task.Id && x.UserId == userId);
                var isOwner = await unitOfWork.Repository<TaskAssignment>().Exists(taskAssignmentSpec, cancellationToken);
                if (!isOwner)
                    return AuthenticationErrors.Forbidden;
            }

            var comment = task.AddComment(request.Content, userId.Value);
            await unitOfWork.Complete(cancellationToken);
            return comment.PublicId;
        }
    }
}
