using Microsoft.EntityFrameworkCore;
using Common.Entities;

namespace TransactionService.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transaction { get; set; } = default!;
    }
}
