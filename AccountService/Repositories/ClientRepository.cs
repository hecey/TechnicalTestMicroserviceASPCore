using AccountService.Data;
using Hecey.TTM.Common.Repositories;

namespace AccountService.Repositories
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
