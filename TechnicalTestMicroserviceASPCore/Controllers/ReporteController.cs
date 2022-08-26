using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalTestMicroserviceASPCore.Data;
using TechnicalTestMicroserviceASPCore.DTOs;

namespace TechnicalTestMicroserviceASPCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : Controller
    {
        private readonly DataContext _context;



        public ReporteController(DataContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<ActionResult<List<ReporteDto>>> Get(DateTime fechaInicio, DateTime fechaFin, string clienteIdentificacion)
        {

            var clientedb = await _context.Cliente.Where(x => x.Identificacion == clienteIdentificacion).FirstOrDefaultAsync();

            if (clientedb == null) return BadRequest("Cliente not found");

            if (fechaInicio == fechaFin)
                fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);

            var movimientos = await _context.Movimiento
                              .Include(c => c.Cuenta.Cliente.Nombre)
                              .Where(c => c.Cuenta.Cliente.Identificacion == clienteIdentificacion
                                            && c.Fecha >= fechaInicio
                                             && c.Fecha <= fechaFin)
                              .ToListAsync();


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
