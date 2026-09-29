using Application.Common.Errors;
using Application.Errors;
using Application.Features.Authentication.DTOs;
using Application.Interfaces.Persistence;
using MediatR;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler(IIdentityService identityService) : IRequestHandler<RefreshTokenCommand, OneOf<LoginDto, InvalidTokenError>>
    {
        public async Task<OneOf<LoginDto, InvalidTokenError>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await identityService.RefreshToken(request.RefreshToken, cancellationToken);
            }
            catch
            {
                return AuthenticationErrors.TokenInvalid;
            }

        }
    }
}
