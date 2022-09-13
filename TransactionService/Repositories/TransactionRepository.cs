using TransactionService.Data;
using Hecey.TTM.Common.Repositories;
using Hecey.TTM.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace TransactionService.Repositories
{
    public class TransactionRepository<T> : Repository<T>, ITransactionRepository<T>, IDisposable where T : class
    {
        protected readonly DataContext _context;

        public TransactionRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<decimal> FindTodaysBalanceUsed(Guid AccountId)
        {
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

            var sumWithdrawn = await DataContext.Transaction
            .Where(x => x.AccountId == AccountId && x.Date.Date == todaysDate.Date
                && x.Amount < 0).SumAsync(x => x.Balance);

            return sumWithdrawn;
        }

        public async Task<IEnumerable<Transaction>> ReportByIDRangeDate(string clientIdentification, DateTime startDate, DateTime endDate)
        {
            return await DataContext.Transaction
                              .Include(c => c.Account.Client.Name)
                              .Where(c => c.Account.Client.Identification == clientIdentification
                                            && c.Date >= startDate
                                             && c.Date <= endDate)
                              .ToListAsync();
        }

        public async Task<Transaction?> LastTransactionByAccount(Guid accountId)
        {
            return await DataContext.Transaction
                              .Where(c => c.AccountId == accountId)
                              .OrderByDescending(x => x.Date).FirstOrDefaultAsync();
        }

        public DataContext DataContext
        {
            get { return (DataContext)Context; }
        }
    }
}
