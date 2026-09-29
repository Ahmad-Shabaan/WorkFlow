
using System.Security.Claims;

namespace Infrastructure.Persistence.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal claimsPrincipal)
        => int.TryParse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) ? userId : null;


        public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Email);


        public static IEnumerable<string> GetRoles(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindAll(ClaimTypes.Role)
                       .Select(c => c.Value);
    }
}
