using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AccountService.Entities
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
        public string? ClientIdentification { get; set; }
        [Required]
        public string? ClientName { get; set; }
    }
}
