
using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Features.Tasks.Commands.CompleteTask
{
    public sealed record CompleteTaskCommand(Guid ProjectId, Guid TaskId) : IRequest<OneOf<Success, NotFoundError>>;
}
