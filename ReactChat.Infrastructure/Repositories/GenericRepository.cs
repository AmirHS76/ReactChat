using Microsoft.EntityFrameworkCore;
using ReactChat.Infrastructure.Extensions;
using System.Linq.Expressions;

namespace ReactChat.Infrastructure.Repositories
{
    public class GenericRepository<T>(DbContext context) : IGenericRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken)
        {
            if (predicate != null)
                return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public void UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public IQueryable<T> GetQuery(T filter)
        {
            return _dbSet.AsQueryable().ApplyFilters<T>(filter);
        }
    }
}
