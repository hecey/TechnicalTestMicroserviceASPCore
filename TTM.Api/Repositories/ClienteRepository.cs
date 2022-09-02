using TTM.Api.Data;
using TTM.Api.Models;

namespace TTM.Api.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository, IDisposable
    {

        public ClienteRepository(DataContext context) : base(context)
        {

        }


    }
}
