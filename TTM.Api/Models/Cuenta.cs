using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TTM.Api.Models
{
    public class Cuenta
    {
        public int Id { get; set; }
        [Required]
        public string? NumeroDeCuenta { get; set; }
        public string? TipoDeCuenta { get; set; }
        [Precision(14, 2)]

        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }
        [JsonIgnore]
        public Cliente Cliente { get; set; } = null!;
        public int ClienteId { get; set; }
    }
}