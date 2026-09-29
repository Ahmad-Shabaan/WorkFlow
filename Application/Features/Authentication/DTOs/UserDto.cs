namespace Application.Features.Authentication.DTOs
{
    public record UserDto(Guid UserId, string DisplayName, string Email)
    {
    }
}

