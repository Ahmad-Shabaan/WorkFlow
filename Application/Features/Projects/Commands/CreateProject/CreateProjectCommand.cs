using Application.Common.Errors;
using MediatR;
using OneOf;
namespace Application.Features.Projects.Commands.CreateProject
{
    public sealed record CreateProjectCommand(string ProjectName, string ProjectDescription, DateTimeOffset StartDate, DateTimeOffset EndDate , Guid ManagerId) : IRequest<OneOf<Guid,NotFoundError, ConflictError, UnauthorizedError>>
    {
    }
}
