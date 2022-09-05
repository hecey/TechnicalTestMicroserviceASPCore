using ClientService.Data;
using Common.Repositories;

namespace ClientService.Repositories
{
    public class ClientRepository<T> : Repository<T>, IClientRepository<T>, IDisposable where T : class
    {
        protected readonly DataContext _context;

        public ClientRepository(DataContext context) : base(context)
        {
            _context = context;

        }
    }
}
