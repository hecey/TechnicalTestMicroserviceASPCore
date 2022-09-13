using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hecey.TTM.Common.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        [Required]
        public string? Number { get; set; }
        public string? Type { get; set; }
        [Precision(14, 2)]
        public decimal InitialBalance { get; set; }
        public bool Status { get; set; }
        [JsonIgnore]
        public Client Client { get; set; } = null!;
        public Guid ClientId { get; set; }

        public ICollection<Transaction>? Transaction { get; set; }

    }
}
