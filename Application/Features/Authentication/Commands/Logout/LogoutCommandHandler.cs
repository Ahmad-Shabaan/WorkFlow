using Application.Common.Errors;
using Application.Errors;
using Application.Interfaces.Persistence;
using MediatR;
using OneOf;

namespace Application.Features.Authentication.Commands.Logout
{
    public class LogoutCommandHandler(IIdentityService identityService) : IRequestHandler<LogoutCommand, OneOf<bool, InvalidTokenError>>
    {
        public async Task<OneOf<bool, InvalidTokenError>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await identityService.Logout(request.RefreshToken, cancellationToken);
                return true;
            }
            catch (Exception)
            {
                return AuthenticationErrors.TokenInvalid;
            }
        }
    }
}
