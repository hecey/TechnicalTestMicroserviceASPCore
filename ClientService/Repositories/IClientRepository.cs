using Hecey.TTM.Common.Repositories;

namespace ClientService.Repositories
{
    public interface IClientRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
    }
}