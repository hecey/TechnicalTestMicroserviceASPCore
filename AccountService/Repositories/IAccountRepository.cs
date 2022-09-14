
using Hecey.TTM.Common.Repositories;

namespace AccountService.Repositories
{
    public interface IAccountRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
    }
}
