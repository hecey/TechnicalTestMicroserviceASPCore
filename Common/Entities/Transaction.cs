using System.ComponentModel.DataAnnotations;

namespace Hecey.TTM.Common.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Type => Amount > 0 ? TransactionType.Deposit : TransactionType.Withdraw;

        public decimal Amount { get; set; } = 0;
        public decimal Balance { get; set; } = 0;
        [Required]
        public string AccountNumber { get; set; } = null!;
    }
    static class TransactionType
    {
        public const string Deposit = "Deposit";
        public const string Withdraw = "Withdraw";
    }
}
