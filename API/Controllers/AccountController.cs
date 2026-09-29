using Application.Features.Authentication.Commands.AssignRole;
using Application.Features.Authentication.Commands.Login;
using Application.Features.Authentication.Commands.Logout;
using Application.Features.Authentication.Commands.RefreshToken;
using Application.Features.Authentication.Commands.Register;
using Application.Features.Authentication.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController(IMediator mediator) : BaseController
    {
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginCommand loginCommand)
        {
            var result = await mediator.Send(loginCommand);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterCommand command)
        {
            await mediator.Send(command);
            return Ok(new { Message = "Account created successfully." });
        }

        [HttpPatch]
        public async Task<ActionResult> RefreshToken(RefreshTokenCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<ActionResult>(loginDto => Ok(loginDto), invalidToken => Unauthorized(invalidToken));
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout(LogoutCommand command)
        {
            var result = await mediator.Send(command);
            return result.Match<ActionResult>(success => Ok(new { Message = "Logged out successfully." }), invalidToken => Unauthorized(invalidToken));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("roles")] // assign-role
        public async Task<ActionResult> AssignRole(AssignRoleCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<ActionResult> GetUsers()
        {
            var result = await mediator.Send(new GetUsersQuery());
            return result.Match<ActionResult>(users => Ok(users), notFoundMsg => NotFound(notFoundMsg));
        }
    }
}
