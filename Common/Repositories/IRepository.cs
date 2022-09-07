using System.Linq.Expressions;

namespace Common.Repositories
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

        void AddAsync(TEntity entity);

        void UpdateAsync(TEntity entity);
        void DeleteAsync(Guid id);
        Task<int> SaveAsync();
    }
}
