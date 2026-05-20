using System.Linq.Expressions;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Repositories
{
    public interface IAsyncRepository<T> where T : EntityBase
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[]? includes);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);

    }
}
