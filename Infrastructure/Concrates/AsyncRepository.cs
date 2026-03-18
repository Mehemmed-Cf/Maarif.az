using Microsoft.EntityFrameworkCore;
using Infrastructure.Abstracts;
using System.Linq.Expressions;

namespace Infrastructure.Concrates
{
    public class AsyncRepository<T> : IAsyncRepository<T>
        where T : class
    {
        protected readonly DbContext db;

        public AsyncRepository(DbContext db)
        {
            this.db = db;
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await db.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<T> EditAsync(T entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            return await Task.FromResult(entity); //direk entity ni gondersek async teleb edir ona gore bu cur edirik
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression = null)
        {
            var query = db.Set<T>().AsQueryable();

            if (expression is not null)
            {
                query = query.Where(expression);
            }

            return query;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            var query = db.Set<T>().AsQueryable();
            T? entity;

            if(expression is not null)
            {
                entity = await query.FirstOrDefaultAsync(expression, cancellationToken);

            } else
            {
                entity = await query.FirstOrDefaultAsync(expression, cancellationToken);
            }

            if (entity == null)
            {
                throw new DirectoryNotFoundException($"{typeof(T).Name} not found");
            }

            return entity;
        }

        public void Remove(T entity)
        {
            db.Remove(entity);
        }

        public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return db.SaveChangesAsync(cancellationToken);
        }
    }
}
