using TechnicalTestMicroserviceASPCore.Models;

namespace TechnicalTestMicroserviceASPCore.Repositories
{
    public interface IMovimientoRepository : IRepository<Movimiento>, IDisposable
    {
        Task<decimal> FindDailyBalanceUsed(int cuentaId);

    }
}
