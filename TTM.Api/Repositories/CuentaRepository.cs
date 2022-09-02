using TTM.Api.Data;
using TTM.Api.Models;

namespace TTM.Api.Repositories
{
    public class CuentaRepository : Repository<Cuenta>, ICuentaRepository, IDisposable
    {
        public CuentaRepository(DataContext context) : base(context)
        {

        }
    }
}
