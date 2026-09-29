using Application.Common.Errors;
using Application.Errors;
using Application.Interfaces.Common;
using Application.Interfaces.Persistence;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using OneOf;
using OneOf.Types;
namespace Application.Features.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<DeleteProjectCommand, OneOf<Success, NotFoundError ,ForbiddenError>>
    {
        public async Task<OneOf<Success, NotFoundError, ForbiddenError>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await unitOfWork.ProjectRepository.Get(request.ProjectId, cancellationToken);
            if (project == null)
                return ProjectErrors.NotFoundError;
            var userId = currentUserService.GetCurrentUserId();
            var roles = currentUserService.GetCurrentUserRoles();
            if (userId is null || roles is null)
                return AuthenticationErrors.UserNotFound;
            if (project.ManagerId != userId && !roles.Any(r => r == UserRoles.Admin.ToString())) // not manager or admin => forbidden
                return AuthenticationErrors.Forbidden;
            unitOfWork.Repository<Project>().Delete(project, cancellationToken);
            await unitOfWork.Complete(cancellationToken);
            return new Success();
        }
    }
}
