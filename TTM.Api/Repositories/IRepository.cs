using System.Linq.Expressions;

namespace TTM.Api.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> Get(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> Find(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> FindAll(
           Expression<Func<TEntity, bool>>? filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
           string includeProperties = "");

        void Add(TEntity entity);

        void Update(TEntity entity);
        void Delete(int id);
        Task Save();
    }
}
