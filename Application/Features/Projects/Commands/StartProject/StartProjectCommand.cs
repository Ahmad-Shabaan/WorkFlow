using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Features.Projects.Commands.StartProject
{
    public sealed record StartProjectCommand(Guid ProjectId) : IRequest<OneOf<Success, NotFoundError>>
    {
    }
}
