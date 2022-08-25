using TechnicalTestMicroserviceASPCore.Data;
using TechnicalTestMicroserviceASPCore.Models;

namespace TechnicalTestMicroserviceASPCore.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository, IDisposable
    {

        public ClienteRepository(DataContext context) : base(context)
        {

        }


    }
}
