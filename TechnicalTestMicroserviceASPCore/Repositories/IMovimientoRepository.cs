using TechnicalTestMicroserviceASPCore.Models;

namespace TechnicalTestMicroserviceASPCore.Repositories
{
    public interface IMovimientoRepository : IRepository<Movimiento>, IDisposable
    {
        Task<decimal> FindTodaysBalanceUsed(int cuentaId);
        Task<IEnumerable<Movimiento>> ReportByIDRangeDate(string clienteIdentificacion, DateTime startDate, DateTime endDate);
        Task<Movimiento?> LastTransactionByAccount(int cuentaId);
    }
}
