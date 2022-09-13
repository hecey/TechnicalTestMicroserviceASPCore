using Hecey.TTM.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientService.Data
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
