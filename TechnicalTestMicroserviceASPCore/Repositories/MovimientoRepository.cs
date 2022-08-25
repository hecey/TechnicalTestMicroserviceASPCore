using Microsoft.EntityFrameworkCore;
using TechnicalTestMicroserviceASPCore.Data;
using TechnicalTestMicroserviceASPCore.Models;

namespace TechnicalTestMicroserviceASPCore.Repositories
{
    public class MovimientoRepository : Repository<Movimiento>, IMovimientoRepository, IDisposable
    {

        public MovimientoRepository(DataContext context) : base(context)
        {

        }

        public async Task<Movimiento> FindLastBalance(int cuentaId)
        {
            var ultimoMovimiento = await Context.Movimiento
                .Where(x => x.CuentaId == cuentaId)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();


            return ultimoMovimiento.First();
        }

        public async Task<decimal> FindSumBalance(int cuentaId)
        {
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var sumaSaldosporFecha = await Context.Movimiento
                .Where(x => x.CuentaId == cuentaId && x.Fecha.Date == todaysDate.Date)
                .SumAsync(x => x.Saldo);

            return sumaSaldosporFecha;
        }
    }
}
