using TechnicalTestMicroserviceASPCore.Data;
using TechnicalTestMicroserviceASPCore.Models;

namespace TechnicalTestMicroserviceASPCore.Repositories
{
    public class CuentaRepository : Repository<Cuenta>, ICuentaRepository, IDisposable
    {
        public CuentaRepository(DataContext context) : base(context)
        {

        }
    }
}
