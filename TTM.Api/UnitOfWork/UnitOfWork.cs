using TTM.Api.Data;
using TTM.Api.Repositories;

namespace TTM.Api.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected readonly DataContext _context;
        public IClienteRepository Clientes { get; }
        public ICuentaRepository Cuentas { get; }
        public IMovimientoRepository Movimientos { get; }

        public UnitOfWork(DataContext context
            , IClienteRepository clienteRepository, ICuentaRepository cuentasRepository
            , IMovimientoRepository movimientoRepository)
        {
            _context = context;
            Clientes = clienteRepository;
            Cuentas = cuentasRepository;
            Movimientos = movimientoRepository;
        }


        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }


}
