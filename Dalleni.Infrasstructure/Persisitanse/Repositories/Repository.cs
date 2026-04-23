using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    /// <summary>
    /// Default EF Core repository implementation.
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<T> DbSet;

        public Repository(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, bool asTracked = true, CancellationToken cancellationToken = default)
        {
            if (asTracked)
            {
                return await DbSet.FindAsync(new object[] { id }, cancellationToken);
            }

            return await GetQuery(false).FirstOrDefaultAsync(BuildIdPredicate(id), cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(bool asTracked = false, CancellationToken cancellationToken = default)
        {
            return await GetQuery(asTracked).ToListAsync(cancellationToken);
        }

        public virtual async Task<IQueryable<T>> GetAllAsQueryableAsync(bool asTracked = false, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return GetQuery(asTracked);
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await DbSet.AddAsync(entity, cancellationToken);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);
            await DbSet.AddRangeAsync(entities, cancellationToken);
        }

        public virtual void Remove(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (entity is ISoftDeletableEntity softDeletable)
            {
                softDeletable.Delete();
                DbSet.Update(entity);
                return;
            }

            DbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }

        public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetQuery(false).AnyAsync(BuildIdPredicate(id), cancellationToken);
        }

        public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await GetQuery(false).CountAsync(cancellationToken);
        }

        protected IQueryable<T> GetQuery(bool asTracked = false)
        {
            return asTracked ? DbSet : DbSet.AsNoTracking();
        }

        protected IQueryable<T> GetQueryWithIncludes(bool asTracked = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = GetQuery(asTracked);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        protected Task<List<T>> FindListAsync(Expression<Func<T, bool>> predicate, bool asTracked = false, CancellationToken cancellationToken = default)
        {
            return GetQuery(asTracked).Where(predicate).ToListAsync(cancellationToken);
        }

        protected Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asTracked = false, CancellationToken cancellationToken = default)
        {
            return GetQuery(asTracked).FirstOrDefaultAsync(predicate, cancellationToken);
        }

        private static Expression<Func<T, bool>> BuildIdPredicate(Guid id)
        {
            var parameter = Expression.Parameter(typeof(T), "entity");
            var property = Expression.Property(parameter, "Id");
            var constant = Expression.Constant(id);
            var body = Expression.Equal(property, constant);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
