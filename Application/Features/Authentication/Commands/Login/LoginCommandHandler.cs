using Application.Features.Authentication.DTOs;
using Application.Interfaces.Persistence;
using MediatR;
namespace Application.Features.Authentication.Commands.Login
{
    public class LoginCommandHandler(IIdentityService identityService) : IRequestHandler<LoginCommand, LoginDto>
    {
        public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        => await identityService.Login(request.Email, request.Password, cancellationToken);
    }
}
