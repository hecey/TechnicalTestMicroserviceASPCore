
namespace Hecey.TTM.Common.Repositories
{
    public interface IAccountRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
    }
}
