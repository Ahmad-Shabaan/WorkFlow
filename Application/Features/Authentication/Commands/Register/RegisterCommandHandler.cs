using Application.Features.Authentication.DTOs;
using Application.Interfaces.Persistence;
using MediatR;
using OneOf.Types;

namespace Application.Features.Authentication.Commands.Register
{
    public class RegisterCommandHandler(IIdentityService identityService) : IRequestHandler<RegisterCommand, Success>
    {
        public async Task<Success> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var registerDto = new RegisterDto(request.FirstName, request.LastName, request.Email, request.Password);
            await identityService.RegisterAsync(registerDto);
            return new Success();
        }
    }
}
