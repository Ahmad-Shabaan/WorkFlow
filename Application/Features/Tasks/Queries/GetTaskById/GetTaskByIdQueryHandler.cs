
using Application.Common.Errors;
using Application.Errors;
using Application.Features.Tasks.DTOs;
using AutoMapper;
using MediatR;
using OneOf;

namespace Application.Features.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler(ITaskQuries taskQuries, IMapper mapper) : IRequestHandler<GetTaskByIdQuery, OneOf<TaskDto, NotFoundError>>
    {
        public async Task<OneOf<TaskDto, NotFoundError>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await taskQuries.GetTaskByIdAsync(request.TaskPublicId, cancellationToken);
            if (task == null) return TaskErrors.NotFoundError;
            var taskDto = mapper.Map<TaskDto>(task);
            return taskDto;
        }
    }
}
