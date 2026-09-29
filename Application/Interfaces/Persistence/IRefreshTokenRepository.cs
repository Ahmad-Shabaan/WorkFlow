
using Domain.Entities;

namespace Application.Interfaces.Persistence
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        public Task<RefreshToken?> GetByTokenAsync(string token);
        System.Threading.Tasks.Task RevokeAllUserTokenAsync(int userId);
    }
}
