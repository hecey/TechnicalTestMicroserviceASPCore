
using Hecey.TTM.Common.Repositories;

namespace AccountService.Repositories
{
    public interface IClientRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
    }
}
