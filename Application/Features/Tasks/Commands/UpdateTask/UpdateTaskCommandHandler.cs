using Application.Common.Errors;
using Application.Errors;
using Application.Features.Projects.Specifications;
using Application.Interfaces.Persistence;
using Domain.Entities.ProjectAggregate;
using MediatR;
using OneOf;
using OneOf.Types;
namespace Application.Features.Tasks.Commands.UpdateTask
{

    public class UpdateTaskCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateTaskCommand, OneOf<Success, NotFoundError>>
    {
        public async Task<OneOf<Success, NotFoundError>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var spec = new GetProjectByPublicIdWithTasks(request.ProjectId);
            var project = await unitOfWork.Repository<Project>().Get(spec, cancellationToken);
            if (project is null)
            {
                return ProjectErrors.NotFoundError;
            }
            project.UpdateTask(request.TaskId, request.TaskName, request.TaskDescription, request.StartDate, request.EndDate);
            return new Success();
        }
    }
}
