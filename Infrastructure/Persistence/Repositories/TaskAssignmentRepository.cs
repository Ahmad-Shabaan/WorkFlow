
using Application.Interfaces.Persistence;
using Application.Specifications;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TaskAssignmentRepository : GenericRepository<TaskAssignment> ,ITaskAssignmentRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<RefreshToken> _dbSet;
        private readonly ISpecificationEvaluator _specificationEvaluator;

        public TaskAssignmentRepository(AppDbContext appDbContext, ISpecificationEvaluator specificationEvaluator) : base(appDbContext, specificationEvaluator)
        {
            _context = appDbContext;
            _dbSet = _context.Set<RefreshToken>();
            _specificationEvaluator = specificationEvaluator;
        }

    }
}
