using Common.Entities;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().Property(o => o.Balance).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Transaction>().Property(o => o.Amount).HasColumnType("decimal(18,4)");

            modelBuilder.Entity<Account>()
            .HasMany<Transaction>(s => s.Transaction)
            .WithOne(g => g.Account)
            .HasForeignKey(s => s.AccountId);
        }
    }

}
