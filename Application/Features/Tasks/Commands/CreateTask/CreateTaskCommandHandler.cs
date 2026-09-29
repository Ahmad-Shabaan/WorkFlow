using Application.Common.Errors;
using Application.Errors;
using Application.Features.Projects.Specifications;
using Application.Interfaces.Persistence;
using Domain.Entities;
using MediatR;
using OneOf;


namespace Application.Features.Tasks.Commands.CreateTask
{
    public sealed class CreateTaskCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateTaskCommand, OneOf<Guid, NotFoundError , ConflictError>>
    {
        public async Task<OneOf<Guid, NotFoundError,ConflictError>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var projectSpec = new GetProjectByPublicIdWithTasks(request.ProjectId);
            var project = await unitOfWork.Repository<Project>().Get(projectSpec, cancellationToken);
            if (project == null)
                return ProjectErrors.NotFoundError;
            foreach (var item in project.Tasks)
            {
                if (request.TaskName == item.TaskName)
                    return ProjectErrors.TaskAlreadyExistError;
            }
            var task = project.AddTask(request.TaskName, request.TaskDescription, request.StartDate, request.EndDate, null);
            await unitOfWork.Complete(cancellationToken);
            return task.PublicId;
        }
    }

}