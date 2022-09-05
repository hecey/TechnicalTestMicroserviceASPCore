using AccountService.Data;
using Common.Repositories;

namespace AccountService.Repositories
{
    public class AccountRepository<T> : Repository<T>, IAccountRepository<T>, IDisposable where T : class
    {
        protected readonly DataContext _context;

        public AccountRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
