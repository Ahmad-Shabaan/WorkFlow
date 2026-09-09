

using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Features.Projects.Commands.DeleteProject
{
    public sealed record DeleteProjectCommand(Guid ProjectId) : IRequest<OneOf<Success, NotFoundError>>
    {
    }
}
