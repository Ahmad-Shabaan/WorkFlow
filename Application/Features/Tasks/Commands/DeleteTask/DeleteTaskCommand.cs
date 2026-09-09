using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;
namespace Application.Features.Tasks.Commands.DeleteTask
{
    public sealed record DeleteTaskCommand(Guid ProjectId, Guid TaskId) : IRequest<OneOf<Success, NotFoundError>>
    {
    }
}
