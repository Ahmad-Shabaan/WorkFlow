using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepository ProjectRepository { get; }
        ITaskRepository TaskRepository { get; }

        public IGenericRepository<Entity> Repository<Entity>() where Entity : class; 
        public Task<int> Complete(CancellationToken cancellationToken);

    }
}
