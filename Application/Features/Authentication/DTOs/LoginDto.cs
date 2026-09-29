namespace Application.Features.Authentication.DTOs
{
    public record LoginDto(string AccessToken, string RefreshToken, DateTime AccessTokenExpiration, DateTime RefreshTokenExpiration)
    {
    }
}
