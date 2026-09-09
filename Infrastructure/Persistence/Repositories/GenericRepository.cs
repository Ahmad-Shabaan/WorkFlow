using Application.Interfaces.Persistence;
using Application.Specifications;
using Domain.Common;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly ISpecificationEvaluator _specificationEvaluator;
        public GenericRepository(AppDbContext appDbContext, ISpecificationEvaluator specificationEvaluator)
        {
            _context = appDbContext;
            _dbSet = _context.Set<T>();
            _specificationEvaluator = specificationEvaluator;
        }

        public void Add(T item) => _dbSet.Add(item);


        public Task BulkDelete(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(T item, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(item);
        }

        public Task<bool> Exists(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<T?> Get(int id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync(id, cancellationToken);
        public async Task<T?> GetByPublicId(Guid publicId, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(e => e.PublicId == publicId, cancellationToken);


        public Task<T?> Get(ISpecification<T> specification, CancellationToken cancellationToken = default)
            => _specificationEvaluator.GetQuery(specification, _dbSet.AsQueryable()).FirstOrDefaultAsync(cancellationToken);

        public Task<TResult?> Get<TResult>(IProjectionSpecification<T, TResult> spec, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<T?> Get(Guid publicId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(e => e.PublicId == publicId).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        }

        public Task<IReadOnlyList<T>> GetAll(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<T>> GetAll(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TResult>> GetAll<TResult>(IProjectionSpecification<T, TResult> spec, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCount(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCount(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Update(T item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


    }
}
