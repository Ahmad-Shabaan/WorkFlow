using Application.Specifications;
namespace Application.Interfaces.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAll(CancellationToken cancellationToken = default);
        public Task<IReadOnlyList<T>> GetAll(ISpecification<T> spec, CancellationToken cancellationToken = default);
        public Task<IReadOnlyList<TResult>> GetAll<TResult>(IProjectionSpecification<T, TResult> spec, CancellationToken cancellationToken = default);
        public Task<T?> Get(int id, CancellationToken cancellationToken = default);
        public Task<T?> Get(Guid publicId, CancellationToken cancellationToken = default);

        public Task<T?> Get(ISpecification<T> specification, CancellationToken cancellationToken = default);
        public Task<TResult?> Get<TResult>(IProjectionSpecification<T, TResult> spec, CancellationToken cancellationToken = default);

        public Task<bool> Exists(ISpecification<T> spec, CancellationToken cancellationToken = default);
        public void Add(T item);
        public void Update(T item, CancellationToken cancellationToken = default);
        public void Delete(T item, CancellationToken cancellationToken = default);
        Task BulkDelete(ISpecification<T> specification, CancellationToken cancellationToken = default);
        public Task<int> GetCount(CancellationToken cancellationToken = default);
        public Task<int> GetCount(ISpecification<T> specification, CancellationToken cancellationToken = default);
        Task<T?> GetByPublicId(Guid publicId, CancellationToken cancellationToken = default);
    }
}
