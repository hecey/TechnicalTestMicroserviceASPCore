namespace Hecey.TTM.Common.Repositories
{
    public interface IClientRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
    }
}