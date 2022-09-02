namespace TTM.Api.Extensions
{
    public static class MovimientoExtensions
    {
        public static string GetTipoDeMovimiento(this Movimiento movimiento)
        {
            return movimiento.Valor > 0 ? "Credito" : "Debito";
        }
    }

    public partial class Movimiento
    {
        public decimal Valor { get; set; }
        public string TipoDeMovimiento
        {
            get { return Valor > 0 ? "Credito" : "Debito"; }
        }
    }
}
