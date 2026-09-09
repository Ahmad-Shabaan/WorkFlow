using Application.Common.Errors;
using MediatR;
using OneOf;


namespace Application.Features.Tasks.Commands.CreateTask
{
    public sealed record CreateTaskCommand(Guid ProjectId, string TaskName, string TaskDescription, DateTimeOffset StartDate, DateTimeOffset EndDate) : IRequest<OneOf<Guid, NotFoundError>>
    {
    }
}
