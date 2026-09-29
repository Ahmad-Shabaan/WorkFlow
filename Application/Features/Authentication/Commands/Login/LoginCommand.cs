using Application.Common.Errors;
using Application.Features.Authentication.DTOs;
using MediatR;
using OneOf;
namespace Application.Features.Authentication.Commands.Login
{
    public sealed record LoginCommand(string Email, string Password) : IRequest<LoginDto>
    {
    }
}
