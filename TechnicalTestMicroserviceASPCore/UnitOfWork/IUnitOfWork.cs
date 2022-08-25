using TechnicalTestMicroserviceASPCore.Repositories;

namespace TechnicalTestMicroserviceASPCore.UnitOfWork
{
    public interface IUnitOfWork
    {
        IClienteRepository Clientes { get; }
        ICuentaRepository Cuentas { get; }
        IMovimientoRepository Movimientos { get; }

        Task<int> Complete();
    }
}
