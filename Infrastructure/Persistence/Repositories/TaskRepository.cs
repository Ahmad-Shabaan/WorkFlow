using Application.Interfaces.Persistence;
using Application.Specifications;
using Dapper;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Task = Domain.Entities.Task;

namespace Infrastructure.Persistence.Repositories
{
    public class TaskRepository : GenericRepository<Task>, ITaskRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Task> _dbSet;
        private readonly ISpecificationEvaluator _specificationEvaluator;

        public TaskRepository(AppDbContext appDbContext, ISpecificationEvaluator specificationEvaluator) : base(appDbContext, specificationEvaluator)
        {
            _context = appDbContext;
            _dbSet = _context.Set<Task>();
            _specificationEvaluator = specificationEvaluator;
        }
        public async Task<IEnumerable<Task>> GetAllAsync(CancellationToken cancellationToken)
        {
            string sql = """
                SELECT * 
                FROM Tasks
                """;
            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
            var result = await _context.Database.GetDbConnection().QueryAsync<Task>(command);
            return result.ToList();
        }

        public async Task<Task?> GetByPublicId(Guid publicId, CancellationToken cancellationToken = default)
         => await _dbSet.FirstOrDefaultAsync(e => e.PublicId == publicId, cancellationToken);

        public async Task<Task?> Get(Guid publicId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(e => e.PublicId == publicId).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        }
        public Task<IEnumerable<Task>> GetAllTasksInProjectAsync(Guid projectPublicId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Task?> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken)
        {
            string sql = """
                SELECT * 
                FROM Tasks
                WHERE PublicId = @TaskId
                """;
            var command = new CommandDefinition(sql, new { TaskId = taskId }, cancellationToken: cancellationToken);
            var result = await _context.Database.GetDbConnection().QueryAsync<Task>(command);
            return result.FirstOrDefault();
        }
    }
}
