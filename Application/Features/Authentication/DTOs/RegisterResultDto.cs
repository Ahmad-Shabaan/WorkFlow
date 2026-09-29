namespace Application.Features.Authentication.DTOs
{

    public record RegisterResultDto(UserDto? User, bool IsEmailUsed, bool Succeeded)
    {
    }
}
