
using Common.Repositories;
using Common.Entities;

namespace Common.Repositories
{
    public interface ITransactionRepository<T> : IRepository<T>, IDisposable  where T : class
    {
        Task<decimal> FindTodaysBalanceUsed(Guid AccountId);
        Task<IEnumerable<Transaction>> ReportByIDRangeDate(string clientIdentification, DateTime startDate, DateTime endDate);
        Task<Transaction?> LastTransactionByAccount(Guid accountId);
    }
}
