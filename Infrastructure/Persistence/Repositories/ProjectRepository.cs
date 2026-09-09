using Application.Interfaces.Persistence;
using Application.Specifications;
using Dapper;
using Domain.Entities.ProjectAggregate;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {

        private readonly AppDbContext _context;
        private readonly DbSet<Project> _dbSet;
        private readonly ISpecificationEvaluator _specificationEvaluator;

        public ProjectRepository(AppDbContext appDbContext, ISpecificationEvaluator specificationEvaluator) : base(appDbContext, specificationEvaluator)
        {
            _context = appDbContext;
            _dbSet = _context.Set<Project>();
            _specificationEvaluator = specificationEvaluator;
        }
    }
}
