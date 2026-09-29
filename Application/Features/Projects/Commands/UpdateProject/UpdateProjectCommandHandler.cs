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
namespace Application.Features.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler(ICurrentUserService currentUserService, IIdentityService identityService, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProjectCommand, OneOf<Success, NotFoundError, ForbiddenError, UnauthorizedError,ConflictError>>
    {
        public async Task<OneOf<Success, NotFoundError, ForbiddenError, UnauthorizedError,ConflictError>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var newManager = await identityService.GetUser(request.ManagerId); // confirm that owner of id is existed
            if (newManager is null)
                return AuthenticationErrors.UserNotFound;
            var project = await unitOfWork.ProjectRepository.GetByPublicId(request.ProjectId, cancellationToken);
            if (project is null)
                return ProjectErrors.NotFoundError;
            var userId = currentUserService.GetCurrentUserId();
            var roles = currentUserService.GetCurrentUserRoles();
            if (userId is null || roles is null)
                return AuthenticationErrors.Unauthorized;

            if (!newManager.Roles.Any(r => r == UserRoles.Manager.ToString()))
                return ProjectErrors.NotManager;
            // check if new manager has porject
            var checkHavingProjectSpec = new GetRecordsByCriteriaSpecification<Project>(x => x.ManagerId == newManager.Id);
            var checkHavingProject = await unitOfWork.Repository<Project>().Exists(checkHavingProjectSpec, cancellationToken);
            if (checkHavingProject)
                return ProjectErrors.MaximumProjects;

            if (roles.Any(r => r == UserRoles.Admin.ToString())) // if user is admin
            {
                // can update who manage project
                project.UpdateProject(request.ProjectName, request.ProjectDescription, request.StartDate, request.EndDate, newManager.Id);
            }
            else
            {
                if (newManager.Id != userId) // another manager try to edit not it's project
                    return AuthenticationErrors.Forbidden;
                // manager update project's info
                project.UpdateProject(request.ProjectName, request.ProjectDescription, request.StartDate, request.EndDate, userId.Value);
            }
            await unitOfWork.Complete(cancellationToken);
            return new Success();
        }
    }
}
