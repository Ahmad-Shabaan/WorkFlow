
using Infrastructure.Options;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Infrastructure.Services
{
    public class JwtService
    {
        private readonly JwtOption _jwtOption;
        public JwtService(IOptions<JwtOption> jwtOption)
        {
            _jwtOption = jwtOption.Value;
        }

        public async Task<(string accessToken, DateTime expiryDate)> GenerateAccessToken(ApplicationUser userApp, UserManager<ApplicationUser> userManager)
        {
            List<Claim> authClaims =
            [
                // Private claims 
                new Claim(ClaimTypes.NameIdentifier, userApp.Id.ToString()),
                new Claim(ClaimTypes.GivenName, userApp.FullName),
                new Claim(ClaimTypes.Email ,userApp.Email!)
            ];
            var userRoles = await userManager.GetRolesAsync(userApp);
            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key));
            var credentials = new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256);
            var accessTokenExpiration = DateTime.UtcNow.AddHours(_jwtOption.ExpiryInHours);
            var jwt = new JwtSecurityToken(
                issuer: _jwtOption.ValidIssuer,
                audience: _jwtOption.ValidAudience,
                claims: authClaims,
                expires: accessTokenExpiration,
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (accessToken, accessTokenExpiration);
        }
    }
}
