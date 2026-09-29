using Application.Common.Errors;
using Application.Errors;
using Application.Features.Projects.Specifications;
using Application.Interfaces.Persistence;
using Application.Specifications;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using OneOf;

namespace Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler(IUnitOfWork unitOfWork, IIdentityService identityService) : IRequestHandler<CreateProjectCommand, OneOf<Guid, NotFoundError, ConflictError,UnauthorizedError>>
    {
        public async Task<OneOf<Guid, NotFoundError, ConflictError,UnauthorizedError>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var user = await identityService.GetUser(request.ManagerId);

            if (user is null)
                return AuthenticationErrors.Unauthorized;
            if (!user.Roles.Any(r => r == UserRoles.Manager.ToString()))
                return ProjectErrors.NotManager;
            var spec = new GetProjectByManagerIdSpecification(user.Id);
            var existingManagedProject = await unitOfWork.Repository<Project>().Exists(spec, cancellationToken);
            if (existingManagedProject)
                return ProjectErrors.ConflictError;
            var project = new Project(request.ProjectName, request.ProjectDescription, request.StartDate, request.EndDate, user.Id);
            unitOfWork.Repository<Project>().Add(project);
            await unitOfWork.Complete(cancellationToken);
            return project.PublicId;
        }
    }
}
