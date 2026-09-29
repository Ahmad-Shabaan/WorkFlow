using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;
namespace Application.Features.Tasks.Commands.AssignToTask
{
    public sealed record AssignToTaskCommand(Guid TaskId, Guid UserId) : IRequest<OneOf<Success, UnauthorizedError, NotFoundError, ForbiddenError, ConflictError>>
    {
    }
}
