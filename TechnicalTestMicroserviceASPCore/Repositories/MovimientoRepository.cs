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

        public async Task<decimal> FindDailyBalanceUsed(int cuentaId)
        {
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var sumaSaldosporFecha = await Context.Movimiento
                .Where(x => x.CuentaId == cuentaId && x.Fecha.Date == todaysDate.Date)
                .SumAsync(x => x.Saldo);

            return sumaSaldosporFecha;
        }
    }
}
