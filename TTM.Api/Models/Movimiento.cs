using System.Text.Json.Serialization;

namespace TTM.Api.Models
{
    public class Movimiento
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string TipoDeMovimiento
        {
            get { return Valor > 0 ? "Credito" : "Debito"; }
        }

        public decimal Valor { get; set; } = 0;
        public decimal Saldo { get; set; } = 0;
        [JsonIgnore]
        public Cuenta Cuenta { get; set; } = null!;
        public int CuentaId { get; set; }

    }
}
