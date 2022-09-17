using TransactionService.Entities;
using Microsoft.EntityFrameworkCore;

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
