using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;
namespace Application.Features.Projects.Commands.UpdateProject
{
    public sealed record UpdateProjectCommand(Guid ProjectId,string ProjectName, string ProjectDescription, DateTimeOffset StartDate, DateTimeOffset EndDate, Guid ManagerId) : IRequest<OneOf<Success, NotFoundError, ForbiddenError, UnauthorizedError,ConflictError>>
    {
    }
}
