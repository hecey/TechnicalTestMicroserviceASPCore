using TransactionService.Entities;
using Hecey.TTM.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using TransactionService.Clients;
using TransactionService.Data;
using TransactionService.DTOs;

namespace TransactionService.Repositories
{
    public class TransactionRepository<T> : Repository<T>, ITransactionRepository<T>, IDisposable where T : class
    {
        protected readonly DataContext _context;
        private readonly RemoteAccountService _remoteAccountService;

        public TransactionRepository(DataContext context, RemoteAccountService remoteAccountService) : base(context)
        {
            _context = context;
            _remoteAccountService = remoteAccountService;
        }

        public async Task<decimal> FindTodaysBalanceUsed(string accountNumber)
        {
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

            var sumWithdrawn = await DataContext.Transaction
            .Where(x => x.AccountNumber == accountNumber && x.Date.Date == todaysDate.Date
                && x.Amount < 0).SumAsync(x => x.Balance);

            return sumWithdrawn;
        }

        public async Task<IEnumerable<ReportDto>> ReportByIDAndRangeDate(string clientIdentification, DateTime startDate, DateTime endDate)
        {


            var accountsDto = await _remoteAccountService.GetAccountsByClientAsync(clientIdentification);
            if (accountsDto == null)
            {
                return null!;
            }

            var accountNumbers = accountsDto.Select(c => c.Number).ToList();


            var transactions = await DataContext.Transaction
                   .Where(c => accountNumbers.Contains(c.AccountNumber)
                                 && c.Date >= startDate
                                  && c.Date <= endDate)
                   .ToListAsync();

            List<ReportDto> reportDtos = new List<ReportDto>();

            AccountDto accountDto = null!;
            foreach (var t in transactions)
            {
                if (accountDto is null || !accountDto.Number!.Equals(t.AccountNumber))
                {
                    accountDto = accountsDto.Where(c => c.Number!.Equals(t.AccountNumber)).First();
                }

                reportDtos.Add(new ReportDto(t.Id, t.Type, t.Amount, t.Balance, t.Date, t.AccountNumber, accountDto.ClientName, accountDto.InitialBalance, accountDto.Status, accountDto.Type));
            }
            return reportDtos;

        }

        public async Task<Transaction?> LastTransactionByAccount(string accountNumber)
        {
            return await DataContext.Transaction
                              .Where(c => c.AccountNumber == accountNumber)
                              .OrderByDescending(x => x.Date).FirstOrDefaultAsync();
        }



        public DataContext DataContext
        {
            get { return (DataContext)Context; }
        }
    }
}
