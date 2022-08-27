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


        public async Task<decimal> FindTodaysBalanceUsed(int cuentaId)
        {
            var todaysDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);


            return await DataContext.Movimiento
            .Where(x => x.CuentaId == cuentaId && x.Fecha.Date == todaysDate.Date)
            .SumAsync(x => x.Saldo);

        }

        public async Task<IEnumerable<Movimiento>> ReportByIDRangeDate(string clienteIdentificacion, DateTime startDate, DateTime endDate)
        {
            return await DataContext.Movimiento
                              .Include(c => c.Cuenta.Cliente.Nombre)
                              .Where(c => c.Cuenta.Cliente.Identificacion == clienteIdentificacion
                                            && c.Fecha >= startDate
                                             && c.Fecha <= endDate)
                              .ToListAsync();
        }


        public async Task<Movimiento?> LastTransactionByAccount(int cuentaId)
        {
            return await DataContext.Movimiento
                              .Where(c => c.CuentaId == cuentaId)
                              .OrderByDescending(x => x.Fecha).FirstOrDefaultAsync();
        }

        public DataContext DataContext
        {
            get { return (DataContext)Context; }
        }
    }
}
