
namespace Common.Repositories
{
    public interface IAccountRepository<T> : IRepository<T>, IDisposable where T : class
    {
    }
}
