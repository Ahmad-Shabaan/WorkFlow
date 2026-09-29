using Application.Interfaces.Persistence;
using Application.Specifications;
using Dapper;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Task = Domain.Entities.Task;

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

        public async Task<Project?> GetByPublicId(Guid publicId, CancellationToken cancellationToken = default)
              => await _dbSet.FirstOrDefaultAsync(e => e.PublicId == publicId, cancellationToken);

        public async Task<Project?> Get(Guid publicId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(e => e.PublicId == publicId).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> GetManagerId(int projectId, CancellationToken cancellationToken)
        => await _dbSet.Where(e => e.Id == projectId).Select(p => p.ManagerId).FirstOrDefaultAsync(cancellationToken);
        
        public async Task<Project?> GetProjectByPublicId(Guid publicId, CancellationToken cancellationToken)
        {
            string sql = """
                SELECT p.Id,p.PublicId,p.ProjectName,p.ProjectDescription,p.StartDate,p.EndDate,p.ProjectStatus,p.ManagerId,t.PublicId,t.TaskName,t.TaskDescription,t.StartDate,t.EndDate,t.TaskStatus,t.StartDate,t.EndDate
                FROM Projects p 
                LEFT JOIN Tasks t ON p.Id = t.ProjectId
                WHERE p.PublicId = @PublicId;
                """;
            var command = new CommandDefinition(sql, new { PublicId = publicId }, cancellationToken: cancellationToken);
            var lookup = new Dictionary<Guid, Project>();
            await _context.Database.GetDbConnection().QueryAsync<Project, Task, Project>(command, (project, task) =>
            {
                if (!lookup.TryGetValue(project.PublicId, out var existingProoject))
                {
                    existingProoject = project;
                    lookup.Add(project.PublicId, existingProoject);
                }
                if (task != null)
                    existingProoject.AddTask(task.TaskName, task.TaskDescription, task.StartDate, task.EndDate, task.PublicId, task.TaskStatus);

                return existingProoject;
            }, splitOn: "PublicId");

            return lookup.Values.FirstOrDefault();
        }

        public async Task<IReadOnlyList<Project>> GetAllProjects(CancellationToken cancellationToken)
        {
            string sql = """
                SELECT * 
                FROM Projects
                """;
            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
            var result = await _context.Database.GetDbConnection().QueryAsync<Project>(command);
            return result.ToList();
        }
    }
}
