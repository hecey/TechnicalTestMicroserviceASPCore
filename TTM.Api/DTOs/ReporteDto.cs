namespace TTM.Api.DTOs
{
    public class ReporteDto
    {
        public DateTime Fecha { get; set; }
        public string? Cliente { get; set; }
        public string? NumeroDeCuenta { get; set; }
        public string? Tipo { get; set; }
        public Decimal SaldoInicial { get; set; }
        public Boolean? Estado { get; set; }

        public Decimal Movimiento { get; set; }

        public Decimal SaldoDisponible { get; set; }


    }
}
