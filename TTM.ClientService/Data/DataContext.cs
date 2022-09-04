using Microsoft.EntityFrameworkCore;
using TTM.Common.Entities;

namespace TTM.ClientService.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Person { get; set; } = default!;
        public DbSet<Client> Client { get; set; } = default!;

    }
}
