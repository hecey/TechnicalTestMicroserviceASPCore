using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TTM.Api.DTOs
{
    public class CuentaDto
    {
        public int Id { get; set; }
        [Required]

        public string? NumeroDeCuenta { get; set; }
        public string? TipoDeCuenta { get; set; }
        [Precision(14, 2)]

        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }

        public int ClienteId { get; set; }


    }
}
