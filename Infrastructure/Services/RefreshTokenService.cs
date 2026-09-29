using Application.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Exceptions;
using Infrastructure.Options;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
namespace Infrastructure.Services
{
    public class RefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly JwtOption _jwtOption;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IOptions<JwtOption> jwtOption, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _jwtOption = jwtOption.Value;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<(string refreshToken, DateTime expiryDate)> GenerateRefreshToken(ApplicationUser userApp, CancellationToken cancellationToken)
        {
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtOption.ExpiryInDays);
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = userApp.Id,
                ExpiryDate = refreshTokenExpiration
            };
            // Save refreshToken to DB
            _refreshTokenRepository.Add(refreshToken);
            await _unitOfWork.Complete(cancellationToken);
            return (refreshToken.Token, refreshTokenExpiration);
        }

        public async Task<(string refreshToken, DateTime expiryDate, ApplicationUser user)> ValidateRefreshToken(string token, CancellationToken cancellationToken)
        {

            var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(token);

            // Could you create type for each of these exceptions and throw them instead of generic exception?
            // but this not a good practice to return information about the token to the client, so we can just return a generic exception message like "Invalid token" or "Token is not valid" instead of specifying the exact reason for failure.
            if (existingRefreshToken == null)
                throw new Exception("Token not found");

            if (existingRefreshToken.IsRevoked)
                throw new Exception("Token revoked");

            if (existingRefreshToken.ExpiryDate < DateTime.UtcNow)
                throw new Exception("Token expired");

            if (existingRefreshToken.IsUsed)
            {
                await _refreshTokenRepository.RevokeAllUserTokenAsync(existingRefreshToken.UserId);
                throw new Exception("Token already used");
            }


            //refrsh token is valide
            // Get user associated with the refresh token
            var user = await _userManager.FindByIdAsync(existingRefreshToken.UserId.ToString());
            if (user == null) throw new UnauthorizedException("User is not authorized.");
            // Create new refresh token
            var result = await GenerateRefreshToken(user, cancellationToken);
            // Mark old token as used
            existingRefreshToken.IsUsed = true;
            existingRefreshToken.ReplacedByToken = result.refreshToken;
            existingRefreshToken.ExpiryDate = result.expiryDate;
            _refreshTokenRepository.Update(existingRefreshToken);
            var rowsAffected = await _unitOfWork.Complete(cancellationToken);
            if (rowsAffected == 0)
                throw new Exception("Failed to revoke refresh token");
            return (result.refreshToken, result.expiryDate, user);
        }

        public async System.Threading.Tasks.Task RevokeRefreshToken(string token, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token) ?? throw new Exception("Refresh token not found");
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            _refreshTokenRepository.Update(refreshToken);
            var rowsAffected = await _unitOfWork.Complete(cancellationToken);
            if (rowsAffected == 0)
                throw new Exception("Failed to revoke refresh token");
        }
    }
}
