using Application.Interfaces.Persistence;
using Application.Specifications;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<RefreshToken> _dbSet;
        private readonly ISpecificationEvaluator _specificationEvaluator;

        public RefreshTokenRepository(AppDbContext appDbContext, ISpecificationEvaluator specificationEvaluator) : base(appDbContext, specificationEvaluator)
        {
            _context = appDbContext;
            _dbSet = _context.Set<RefreshToken>();
            _specificationEvaluator = specificationEvaluator;
        }
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _dbSet.FirstOrDefaultAsync(rt => rt.Token == token);
        }
        public async System.Threading.Tasks.Task RevokeAllUserTokenAsync(int userId)
        {
            await _dbSet.Where(rt => rt.UserId == userId && !rt.IsRevoked)
                        .ExecuteUpdateAsync(setters => setters
                        .SetProperty(rt => rt.IsUsed, true)
                        .SetProperty(rt => rt.IsRevoked, true)
                        .SetProperty(rt => rt.RevokedAt, DateTime.UtcNow));
        }
    }
}
