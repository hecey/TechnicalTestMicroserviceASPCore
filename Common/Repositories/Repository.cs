using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Repositories
{

    public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }

        public async Task<TEntity?> GetAsync(Guid id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }
        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }
        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();
        }

        public async virtual Task<IEnumerable<TEntity>> FindAsync(
           Expression<Func<TEntity, bool>>? filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
           string includeProperties = "")
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }


        public void AddAsync(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public async void DeleteAsync(Guid id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
                Context.Set<TEntity>().Remove(entity);
        }

        public void UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;

        }


        public async Task<int> SaveAsync()
        {
            return await Context.SaveChangesAsync();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
