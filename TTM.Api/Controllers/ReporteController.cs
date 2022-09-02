using Microsoft.AspNetCore.Mvc;
using TTM.Api.DTOs;
using TTM.Api.UnitOfWork;

namespace TTM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReporteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReporteDto>>> Get(DateTime fechaInicio, DateTime fechaFin, string clienteIdentificacion)
        {

            var clientedb = await _unitOfWork.Clientes.Find(x => x.Identificacion == clienteIdentificacion);

            if (clientedb == null) return BadRequest("Cliente not found");

            if (fechaInicio == fechaFin)
                fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);

            var movimientos = await _unitOfWork.Movimientos.ReportByIDRangeDate(clienteIdentificacion, fechaInicio, fechaFin);

            List<ReporteDto> reporte = new List<ReporteDto>();

            foreach (var r in movimientos)
            {
                reporte.Add(new ReporteDto()
                {
                    Fecha = r.Fecha,
                    Cliente = r.Cuenta.Cliente.Nombre,
                    NumeroDeCuenta = r.Cuenta.NumeroDeCuenta,
                    Tipo = r.Cuenta.TipoDeCuenta,
                    SaldoInicial = r.Cuenta.SaldoInicial,
                    Estado = r.Cuenta.Estado,
                    Movimiento = r.Valor,
                    SaldoDisponible = r.Saldo

                });

            }



            return Ok(reporte);

        }

    }
}
