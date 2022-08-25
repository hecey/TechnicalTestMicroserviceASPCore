using TechnicalTestMicroserviceASPCore.Models;

namespace TechnicalTestMicroserviceASPCore.Repositories
{
    public interface IMovimientoRepository : IRepository<Movimiento>, IDisposable
    {
        Task<Movimiento> FindLastBalance(int cuentaId);

        Task<decimal> FindSumBalance(int cuentaId);

    }
}
