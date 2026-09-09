using Application.Common.Errors;
using Application.Errors;
using Application.Features.Projects.Specifications;
using Application.Interfaces.Persistence;
using Domain.Entities.ProjectAggregate;
using MediatR;
using OneOf;
using OneOf.Types;


namespace Application.Features.Tasks.Commands.CompleteTask
{
    public class CompleteTaskCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CompleteTaskCommand, OneOf<Success, NotFoundError>>
    {
        public async Task<OneOf<Success, NotFoundError>> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetProjectByPublicIdWithTasks(request.ProjectId);
            var project = await unitOfWork.Repository<Project>().Get(spec, cancellationToken);
            if (project == null)
                return ProjectErrors.NotFoundError;

            project.CompleteTask(request.TaskId);
            await unitOfWork.Complete(cancellationToken);
            return new Success();
        }
    }
}
