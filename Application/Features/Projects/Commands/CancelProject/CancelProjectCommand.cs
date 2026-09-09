using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Features.Projects.Commands.CancelProject
{
    public sealed record CancelProjectCommand(Guid ProjectId) : IRequest<OneOf<Success, NotFoundError>>
    {
    }
}
