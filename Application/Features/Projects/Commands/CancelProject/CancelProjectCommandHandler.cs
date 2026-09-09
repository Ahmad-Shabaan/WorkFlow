using Application.Common.Errors;
using Application.Errors;
using Application.Features.Projects.Specifications;
using Application.Interfaces.Persistence;
using Domain.Entities.ProjectAggregate;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Features.Projects.Commands.CancelProject
{
    public class CancelProjectCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CancelProjectCommand, OneOf<Success, NotFoundError>>
    {
        public async Task<OneOf<Success, NotFoundError>> Handle(CancelProjectCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetProjectByPublicIdWithTasks(request.ProjectId);
            var project = await unitOfWork.Repository<Project>().Get(spec, cancellationToken);
            if (project == null)
                return ProjectErrors.NotFoundError;
            project.Cancel();
            await unitOfWork.Complete(cancellationToken);
            return new Success();
        }
    }
}
