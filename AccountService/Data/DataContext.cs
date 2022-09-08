using Microsoft.EntityFrameworkCore;
using Common.Entities;

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
