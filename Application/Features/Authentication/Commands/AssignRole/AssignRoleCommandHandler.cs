using Application.Interfaces.Persistence;
using MediatR;
using OneOf;
using OneOf.Types;
namespace Application.Features.Authentication.Commands.AssignRole
{
    public class AssignRoleCommandHandler(IIdentityService identityService) : IRequestHandler<AssignRoleCommand, OneOf<Success>>
    {
        public async Task<OneOf<Success>> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
        {
            await identityService.AssignRole(request.UserId, request.RoleId);
            return new Success();
        }
    }
}
