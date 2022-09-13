using Microsoft.EntityFrameworkCore;
using Hecey.TTM.Common.Entities;

namespace AccountService.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Account { get; set; } = default!;
    }
}
