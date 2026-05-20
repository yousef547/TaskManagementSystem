

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly ApplicationDbContext _contextDb;

        public RepositoryBase(ApplicationDbContext contextDb)
        {
            _contextDb = contextDb;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _contextDb.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _contextDb.Set<T>();

            query = query.Where(predicate);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id,params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = _contextDb.Set<T>();

            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> AddAsync(T entity ,CancellationToken cancellationToken)
        {
            _contextDb.Set<T>().Add(entity);
            await _contextDb.SaveChangesAsync(cancellationToken);
            return entity;
        }
        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            _contextDb.Entry(entity).State = EntityState.Modified;
            _contextDb.Set<T>().Update(entity);
            await _contextDb.SaveChangesAsync(cancellationToken);
        }


        public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            _contextDb.Set<T>().Remove(entity);
            await _contextDb.SaveChangesAsync(cancellationToken);
        }

    }
}
