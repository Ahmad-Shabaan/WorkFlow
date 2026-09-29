
using Application.Common.Errors;
using Application.Errors;
using Application.Features.Projects.Specifications;
using Application.Interfaces.Persistence;
using Domain.Entities;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Features.Projects.Commands.CompleteProject
{
    public class CompleteProjectCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CompleteProjectCommand, OneOf<Success, NotFoundError>>
    {
        public async Task<OneOf<Success, NotFoundError>> Handle(CompleteProjectCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetProjectByPublicIdWithTasks(request.ProjectId);
            var project = await unitOfWork.Repository<Project>().Get(spec, cancellationToken);
            if (project == null)
                return ProjectErrors.NotFoundError;
            project.Complete();
            await unitOfWork.Complete(cancellationToken);
            return new Success();
        }
    }
}
