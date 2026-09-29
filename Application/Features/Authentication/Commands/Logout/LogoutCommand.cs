using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Features.Authentication.Commands.Logout
{
    public sealed record LogoutCommand(string RefreshToken) : IRequest<OneOf<bool, InvalidTokenError>>
    {
    }
}
