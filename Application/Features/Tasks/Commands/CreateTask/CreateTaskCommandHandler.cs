using Application.Common.Errors;
using Application.Errors;
using Application.Features.Projects.Queries;
using Application.Interfaces.Persistence;
using Domain.Entities.ProjectAggregate;
using MediatR;
using OneOf;


namespace Application.Features.Tasks.Commands.CreateTask
{
    public sealed class CreateTaskCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateTaskCommand, OneOf<Guid, NotFoundError>>
    {
        public async Task<OneOf<Guid, NotFoundError>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {

            var project = await unitOfWork.Repository<Project>().GetByPublicId(request.ProjectId,cancellationToken);
            if (project == null)
                return ProjectErrors.NotFoundError;

            var task = project.AddTask(request.TaskName, request.TaskDescription, request.StartDate, request.EndDate,null);
            await unitOfWork.Complete(cancellationToken);
            return task.PublicId;
        }
    }

}