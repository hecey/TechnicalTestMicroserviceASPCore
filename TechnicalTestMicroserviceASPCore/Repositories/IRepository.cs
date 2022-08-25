using System.Linq.Expressions;

namespace TechnicalTestMicroserviceASPCore.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> Get(int id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void Update(TEntity entity);
        void Delete(int id);
        Task Save();
    }
}
