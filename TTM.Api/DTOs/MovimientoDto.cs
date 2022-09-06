namespace TTM.Api.DTOs
{
    public class MovimientoDto
    {
        public int Id { get; set; }

        public string? TipoDeMovimiento { get; set; }

        public decimal Valor { get; set; } = 0;
        public decimal Saldo { get; set; } = 0;
        public int CuentaId { get; set; }

        public string? NumeroDeCuenta { get; set; }
    }
}
