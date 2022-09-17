using TransactionService.Entities;
using Hecey.TTM.Common.Repositories;
using TransactionService.DTOs;

namespace TransactionService.Repositories
{
    public interface ITransactionRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        Task<decimal> FindTodaysBalanceUsed(string accountNumber);
        Task<IEnumerable<ReportDto>> ReportByIDAndRangeDate(string clientIdentification, DateTime startDate, DateTime endDate);
        Task<Transaction?> LastTransactionByAccount(string accountNumber);
    }
}
