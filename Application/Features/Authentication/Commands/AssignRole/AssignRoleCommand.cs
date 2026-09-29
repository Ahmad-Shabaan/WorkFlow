using Domain.Enums;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Features.Authentication.Commands.AssignRole
{
    public sealed record AssignRoleCommand(Guid UserId , int RoleId ) : IRequest<OneOf<Success>>
    {
    }
}
