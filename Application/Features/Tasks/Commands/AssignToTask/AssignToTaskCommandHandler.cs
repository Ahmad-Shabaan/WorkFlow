using Application.Common.Errors;
using Application.Errors;
using Application.Interfaces.Common;
using Application.Interfaces.Persistence;
using Application.Specifications;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using OneOf;
using OneOf.Types;
using TaskStatus = Domain.Enums.TaskStatus;
namespace Application.Features.Tasks.Commands.AssignToTask
{
    internal class AssignToTaskCommandHandler(IUnitOfWork unitOfWork, IIdentityService identityService, ICurrentUserService currentUserService) : IRequestHandler<AssignToTaskCommand, OneOf<Success, UnauthorizedError, NotFoundError, ForbiddenError, ConflictError>>
    {
        async System.Threading.Tasks.Task<OneOf<Success, UnauthorizedError, NotFoundError, ForbiddenError, ConflictError>> IRequestHandler<AssignToTaskCommand, OneOf<Success, UnauthorizedError, NotFoundError, ForbiddenError, ConflictError>>.Handle(AssignToTaskCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = currentUserService.GetCurrentUserId();
            if (currentUserId is null)
                return AuthenticationErrors.Unauthorized;



            var task = await unitOfWork.TaskRepository.GetByPublicId(request.TaskId, cancellationToken);
            if (task is null)
                return TaskErrors.NotFoundError;

            var project = await unitOfWork.Repository<Project>().Get(task.ProjectId, cancellationToken);
            if (project is null)
                return ProjectErrors.NotFoundError;

            if (project.ManagerId != currentUserId) // not manager who try to assing user to taks
                return AuthenticationErrors.Forbidden;


            var userInfo = await identityService.GetUser(request.UserId);
            if (userInfo is null)
                return AuthenticationErrors.UserNotFound;

            if (!userInfo.Roles.Any(r => r == UserRoles.Employee.ToString()))
                return TaskErrors.UserNotEmployee; 


            var spec = new GetRecordsByCriteriaSpecification<TaskAssignment>(x => x.TaskId == task.Id && x.UserId == userInfo.Id); // ta.userid = userid 
            var exists = await unitOfWork.Repository<TaskAssignment>().Exists(spec, cancellationToken);
            if (exists)
                return TaskErrors.AlreadyAssigned;;


            var activeTasksCountSpec = new GetRecordsByCriteriaSpecification<TaskAssignment>
                (x => x.UserId == userInfo.Id && (
                x.Task.TaskStatus == TaskStatus.InProgress ||
                x.Task.TaskStatus == TaskStatus.InReview ||
                x.Task.TaskStatus == TaskStatus.Todo
            ));
            var activeTasksCount = await unitOfWork.Repository<TaskAssignment>().GetCount(activeTasksCountSpec, cancellationToken);

            if (activeTasksCount >= 2)
                return TaskErrors.MaximumActiveTasks; //cannot be assigned to more than 2 active tasks



            var taskAssignment = new TaskAssignment() { TaskId = task.Id, UserId = userInfo.Id };
            unitOfWork.Repository<TaskAssignment>().Add(taskAssignment);
            await unitOfWork.Complete(cancellationToken);
            return new Success();
        }
    }
}
