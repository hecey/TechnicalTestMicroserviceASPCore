using System.Linq.Expressions;

namespace Hecey.TTM.Common.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAsync();
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> FindAsync(
           Expression<Func<TEntity, bool>>? filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
           string includeProperties = "");

        void Add(TEntity entity);

        void Update(TEntity entity);
        void Delete(Guid id);
        Task<int> SaveAsync();
    }
}
