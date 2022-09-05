namespace Common.Repositories
{
    public interface IClientRepository<T> : IRepository<T>, IDisposable where T : class
    {
    }
}