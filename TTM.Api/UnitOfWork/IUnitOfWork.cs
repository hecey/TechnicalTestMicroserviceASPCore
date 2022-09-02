using TTM.Api.Repositories;

namespace TTM.Api.UnitOfWork
{
    public interface IUnitOfWork
    {
        IClienteRepository Clientes { get; }
        ICuentaRepository Cuentas { get; }
        IMovimientoRepository Movimientos { get; }

        Task<int> Complete();
    }
}
