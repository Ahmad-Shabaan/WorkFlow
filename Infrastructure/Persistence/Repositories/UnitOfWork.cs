using Application.Interfaces.Persistence;
using Domain.Common;
using Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork(AppDbContext context, IProjectRepository projectRepository,ITaskRepository taskRepository, IServiceProvider serviceProvider) : IUnitOfWork
    {
        public IProjectRepository ProjectRepository { get; } = projectRepository;
        public ITaskRepository TaskRepository { get; } = taskRepository;

        private readonly Dictionary<Type, object> _repositories = [];
        public async Task<int> Complete(CancellationToken cancellationToken)
        {
            var result = await context.SaveChangesAsync(cancellationToken);
            return result;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IGenericRepository<Entity> Repository<Entity>() where Entity : class
        {
            var type = typeof(Entity);
            if (!_repositories.TryGetValue(type, out var repository))
            {
                /// create instance from GenericRepository
                //repository = Activator.CreateInstance(typeof(GenericRepository<>).MakeGenericType(type), context)!;
                repository = serviceProvider.GetRequiredService<IGenericRepository<Entity>>();
                _repositories.Add(type, repository);
            }
            return (IGenericRepository<Entity>)repository;
        }
    }
}
