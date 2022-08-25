using Microsoft.AspNetCore.Mvc;
using TechnicalTestMicroserviceASPCore.DTOs;
using TechnicalTestMicroserviceASPCore.Models;
using TechnicalTestMicroserviceASPCore.Repositories;

namespace TechnicalTestMicroserviceASPCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosController : Controller
    {

        private readonly IConfiguration _configRoot;

        private IMovimientoRepository _movimientoRepository;
        private ICuentaRepository _cuentaRepository;


        public MovimientosController(IMovimientoRepository movimientoRepository, ICuentaRepository cuentaRepository, IConfiguration configRoot)
        {
            _movimientoRepository = movimientoRepository;
            _cuentaRepository = cuentaRepository;
            _configRoot = (IConfigurationRoot)configRoot;

        }


        [HttpGet]
        public async Task<ActionResult<List<MovimientoDto>>> Get()
        {
            var movimiento = await _movimientoRepository.GetAll();


            return Ok(movimiento);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovimientoDto>> FindMovimiento(int id)
        {

            var movimiento = await _movimientoRepository.Get(id);
            if (movimiento == null)
            {
                return BadRequest("Movimiento not found");
            }
            return Ok(movimiento);

        }

        [HttpPost]
        public async Task<ActionResult<List<MovimientoDto>>> AddMovimiento(MovimientoDto movimientoDto)
        {
            if (movimientoDto is null)
            {
                return BadRequest("No Movimiento to add");
            }


            var movimientos = await _movimientoRepository.Find(x => x.Id == movimientoDto.Id);
            if (movimientos.Any())
            {
                return BadRequest("Already in database");
            }

            var cuentas = await _cuentaRepository.Find(x => x.NumeroDeCuenta == movimientoDto.NumeroDeCuenta);
            if (!cuentas.Any())
            {
                return BadRequest("Cuenta not found in database");
            }

            if (movimientoDto.Valor == 0)
            {
                return BadRequest("Valor es 0");
            }

            // Los valores cuando son crédito son positivos, y los débitos son negativos.
            // Debe almacenarse el saldo disponible en cada transacción dependiendo del
            // tipo de movimiento.
            movimientoDto.TipoDeMovimiento = movimientoDto.Valor > 0 ? "credito" : "debito";

            // Si el saldo es menor a la transacción débito, debe desplegar mensaje
            // “Saldo no disponible”
            decimal saldoDisponible = 0;
            decimal saldoAnterior = 0;

            Movimiento ultimoMovimientoCliente = await _movimientoRepository.FindLastBalance(cuentas.First().Id);


            saldoAnterior = ultimoMovimientoCliente is not null ?
                              ultimoMovimientoCliente.Saldo :
                              cuentas.First().SaldoInicial;

            if (movimientoDto.TipoDeMovimiento.Equals("debito"))
            {

                if (saldoAnterior + movimientoDto.Valor < 0)
                {
                    return BadRequest("Saldo no disponible");
                }


            }


            saldoDisponible = saldoAnterior + movimientoDto.Valor;


            // Se debe tener un parámetro de limite diario de retiro (valor tope 1000$)

            var LimiteDiario = _configRoot.GetValue<decimal>("LimiteDiario");

            if (LimiteDiario < 0)
            {
                return BadRequest("No se ha definido limite diario en el sistema");
            }



            var sumaSaldosCuentaHoy = await _movimientoRepository.FindSumBalance(cuentas.First().Id);


            if (sumaSaldosCuentaHoy >= LimiteDiario)
            {
                return BadRequest("Cupo diario Excedido");
            }

            //Guardar 

            var MovimientoNueva = new Movimiento
            {
                Fecha = DateTime.Now,
                TipoDeMovimiento = movimientoDto.TipoDeMovimiento,
                Valor = movimientoDto.Valor,
                Saldo = saldoDisponible,
                CuentaId = cuentas.First().Id,
                Cuenta = cuentas.First()
            };

            _movimientoRepository.Add(MovimientoNueva);
            await _movimientoRepository.Save();

            return Ok(await _movimientoRepository.GetAll());
        }

        [HttpPut]
        public async Task<ActionResult<MovimientoDto>> UpdateMovimiento(MovimientoDto movimientoDtoUpdate)
        {
            if (movimientoDtoUpdate == null)
            {
                return BadRequest("No Movimiento to add");
            }
            var movimientos = await _movimientoRepository.Find(x => x.Id == movimientoDtoUpdate.Id);
            if (movimientos == null)
            {
                return BadRequest("Movimiento not in database");
            }
            var movimientodb = movimientos.First();

            movimientodb.Fecha = DateTime.Now;
            movimientodb.TipoDeMovimiento = movimientoDtoUpdate.TipoDeMovimiento;
            movimientodb.Valor = movimientoDtoUpdate.Valor;
            movimientodb.Saldo = movimientoDtoUpdate.Saldo;
            movimientodb.CuentaId = movimientoDtoUpdate.CuentaId;

            await _movimientoRepository.Save();

            return Ok(movimientodb);
        }

        [HttpDelete]
        public async Task<ActionResult<List<MovimientoDto>>> RemoveMovimiento(MovimientoDto movimientoDto)
        {
            var movimientodb = await _movimientoRepository.Find(x => x.Id == movimientoDto.Id);
            if (movimientodb == null)
            {
                return BadRequest("Movimiento not found");
            }
            _movimientoRepository.Delete(movimientodb.First().Id);
            await _movimientoRepository.Save();
            return Ok(await _movimientoRepository.GetAll());
        }
    }
}
