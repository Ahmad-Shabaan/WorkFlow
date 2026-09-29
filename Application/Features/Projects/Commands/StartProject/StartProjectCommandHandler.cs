using Application.Common.Errors;
using Application.Errors;
using Application.Features.Projects.Specifications;
using Application.Interfaces.Common;
using Application.Interfaces.Persistence;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Features.Projects.Commands.StartProject
{
    public class StartProjectCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService) : IRequestHandler<StartProjectCommand, OneOf<Success, NotFoundError,ForbiddenError>>
    {
        public async Task<OneOf<Success, NotFoundError,ForbiddenError>> Handle(StartProjectCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetProjectByPublicIdWithTasks(request.ProjectId);
            var project = await unitOfWork.Repository<Project>().Get(spec, cancellationToken);
            if (project == null)
                return ProjectErrors.NotFoundError;
            var userId = currentUserService.GetCurrentUserId();
            if (userId is null)
                return AuthenticationErrors.UserNotFound;
            if (project.ManagerId != userId)
                return AuthenticationErrors.Forbidden;
            project.Start();
            await unitOfWork.Complete(cancellationToken);
            return new Success();
        }
    }
}
