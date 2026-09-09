using Application.Features.Tasks.DTOs;
using MediatR;

namespace Application.Features.Tasks.Queries.GetTasks
{
    public sealed record GetTasksQuery : IRequest<IEnumerable<TaskDto>>
    {
    }
}
