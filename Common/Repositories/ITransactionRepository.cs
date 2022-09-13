using Hecey.TTM.Common.Entities;

namespace Hecey.TTM.Common.Repositories
{
    public interface ITransactionRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        Task<decimal> FindTodaysBalanceUsed(Guid AccountId);
        Task<IEnumerable<Transaction>> ReportByIDRangeDate(string clientIdentification, DateTime startDate, DateTime endDate);
        Task<Transaction?> LastTransactionByAccount(Guid accountId);
    }
}
