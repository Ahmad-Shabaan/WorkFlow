
using Application.Common.Errors;
using Application.Errors;
using Application.Features.Tasks.DTOs;
using Application.Interfaces.Persistence;
using Application.Specifications;
using AutoMapper;
using Domain.Entities;
using MediatR;
using OneOf;

namespace Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler(IIdentityService identityService, IUnitOfWork unitOfWork, ITaskRepository taskRepository, IMapper mapper) : IRequestHandler<GetTaskByIdQuery, OneOf<TaskResponseDto, NotFoundError>>
    {
        public async Task<OneOf<TaskResponseDto, NotFoundError>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.GetTaskByIdAsync(request.TaskPublicId, cancellationToken);
            if (task == null) return TaskErrors.NotFoundError;

            var spec = new GetRecordsByCriteriaSpecification<TaskAssignment>(x => x.TaskId == task.Id);
            var taskAssignments = await unitOfWork.Repository<TaskAssignment>().GetAll(spec, cancellationToken);
            var usersIds = taskAssignments.Select(x => x.UserId).ToList();
            var users = await identityService.GetTargetUsers(usersIds);
            return new TaskResponseDto(task.PublicId, task.TaskName, task.TaskDescription, task.StartDate, task.EndDate, task.TaskStatus.ToString(), [.. users]);
        }
    }
}
