using Microsoft.EntityFrameworkCore;
using TechnicalTestMicroserviceASPCore.Models;

namespace TechnicalTestMicroserviceASPCore.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Persona> Persona { get; set; } = default!;
        public DbSet<Cliente> Cliente { get; set; } = default!;
        public DbSet<Cuenta> Cuenta { get; set; } = default!;
        public DbSet<Movimiento> Movimiento { get; set; } = default!;


    }
}
