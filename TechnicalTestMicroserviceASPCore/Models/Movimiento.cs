using System.Text.Json.Serialization;

namespace TechnicalTestMicroserviceASPCore.Models
{
    public class Movimiento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? TipoDeMovimiento { get; set; }

        public decimal Valor { get; set; } = 0;
        public decimal Saldo { get; set; } = 0;
        [JsonIgnore]
        public Cuenta Cuenta { get; set; } = null!;
        public int CuentaId { get; set; }

    }
}
