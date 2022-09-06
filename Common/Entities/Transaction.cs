using System.Text.Json.Serialization;

namespace Common.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Type
        {
            get { return Amount > 0 ? TransactionType.Deposit : TransactionType.Withdraw; }
        }
        public decimal Amount { get; set; } = 0;
        public decimal Balance { get; set; } = 0;
        [JsonIgnore]
        public Account Account { get; set; } = null!;
        public Guid AccountId { get; set; }
    }
    static class TransactionType
    {
        public const string Deposit = "Deposit";
        public const string Withdraw = "Withdraw";
    }
}
