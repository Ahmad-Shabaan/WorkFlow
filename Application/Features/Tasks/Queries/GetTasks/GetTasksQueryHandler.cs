using Application.Features.Tasks.DTOs;
using AutoMapper;
using MediatR;


namespace Application.Features.Tasks.Queries.GetTasks
{
    public class GetTasksQueryHandler(ITaskQuries taskQuries, IMapper mapper) : IRequestHandler<GetTasksQuery, IEnumerable<TaskDto>>
    {
        public async Task<IEnumerable<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await taskQuries.GetAllAsync(cancellationToken);
            var taskDtos = mapper.Map<IEnumerable<TaskDto>>(tasks);
            return taskDtos;
        }
    }
}
