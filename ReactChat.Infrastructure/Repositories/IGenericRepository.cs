using System.Linq.Expressions;

namespace ReactChat.Infrastructure.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        void UpdateAsync(T entity);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        IQueryable<T> GetQuery(T filter);
    }
}
