using Application.Common.Errors;
using Application.Features.Authentication.DTOs;
using MediatR;
using OneOf;

namespace Application.Features.Authentication.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<OneOf<LoginDto, InvalidTokenError>>
    {
    }
}
