using MediatR;
using OneOf.Types;
namespace Application.Features.Authentication.Commands.Register
{
    public sealed record RegisterCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Success>
    {
    }
}
