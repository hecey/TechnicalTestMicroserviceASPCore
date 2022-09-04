using TTM.ClientService.Data;
using TTM.Common.Repositories;

namespace TTM.ClientService.Repositories
{
    public class ClientRepository<T> : Repository<T>, IClientRepository<T>, IDisposable where T : class
    {
        public ClientRepository(DataContext context) : base(context)
        {
        }
    }
}
