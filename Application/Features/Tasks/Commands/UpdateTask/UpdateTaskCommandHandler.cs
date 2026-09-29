using Application.Common.Errors;
using Application.Errors;
using Application.Features.Projects.Specifications;
using Application.Interfaces.Common;
using Application.Interfaces.Persistence;
using Domain.Entities;
using MediatR;
using OneOf;
using OneOf.Types;
namespace Application.Features.Tasks.Commands.UpdateTask
{

    public class UpdateTaskCommandHandler(IUnitOfWork unitOfWork , ICurrentUserService currentUserService) : IRequestHandler<UpdateTaskCommand, OneOf<Success, NotFoundError,UnauthorizedError,ForbiddenError>>
    {
        public async Task<OneOf<Success, NotFoundError,UnauthorizedError,ForbiddenError>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetProjectByPublicIdWithTasks(request.ProjectId);
            var project = await unitOfWork.Repository<Project>().Get(spec, cancellationToken);
            if (project is null)
            {
                return ProjectErrors.NotFoundError;
            }
            var userId = currentUserService.GetCurrentUserId();
            if (userId is null)
                return AuthenticationErrors.Unauthorized;
            if (project.ManagerId != userId)
                return AuthenticationErrors.Forbidden;
            project.UpdateTask(request.TaskId, request.TaskName, request.TaskDescription, request.StartDate, request.EndDate);
            await unitOfWork.Complete(cancellationToken);
            return new Success();
        }
    }
}
