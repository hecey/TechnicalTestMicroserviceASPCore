using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TTM.Api.DTOs;
using TTM.Api.Models;
using TTM.Api.UnitOfWork;

namespace TTM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosController : Controller
    {

        private readonly IConfiguration _configRoot;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovimientosController(IUnitOfWork unitOfWork, IConfiguration configRoot, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configRoot = (IConfigurationRoot)configRoot;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<List<MovimientoDto>>> Get()
        {
            var movimientos = await _unitOfWork.Movimientos.GetAll();
            var movimientosDto = _mapper.Map<IEnumerable<MovimientoDto>>(movimientos);

            return movimientosDto.Any() ? Ok(movimientosDto) : NoContent();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovimientoDto>> FindMovimiento(int id)
        {

            var movimiento = await _unitOfWork.Movimientos.Get(id);
            if (movimiento == null)
            {
                return BadRequest("Movimiento not found");
            }
            var movimientoDto = _mapper.Map<MovimientoDto>(movimiento);

            return Ok(movimientoDto);

        }

        [HttpPost]
        public async Task<ActionResult<List<MovimientoDto>>> AddMovimiento(MovimientoDto movimientoDto)
        {
            if (movimientoDto is null)
            {
                return BadRequest("No Movimiento to add");
            }


            var movimiento = await _unitOfWork.Movimientos.Find(x => x.Id == movimientoDto.Id);
            if (movimiento != null)
            {
                return BadRequest("Already in database");
            }

            var cuenta = await _unitOfWork.Cuentas.Find(x => x.NumeroDeCuenta == movimientoDto.NumeroDeCuenta);
            if (cuenta == null)
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

            // Si el saldo es menor a la transacción débito, debe desplegar mensaje
            // “Saldo no disponible”
            decimal saldoDisponible = 0;
            decimal saldoAnterior = 0;

            var ultimoMovimiento = await _unitOfWork.Movimientos.LastTransactionByAccount(cuenta.Id);

            saldoAnterior = ultimoMovimiento is not null ?
                              ultimoMovimiento.Saldo :
                              cuenta.SaldoInicial;

            if (movimientoDto.Valor < 0)
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

            var sumaRetirosHoy = await _unitOfWork.Movimientos.FindTodaysBalanceUsed(cuenta.Id);

            if ((LimiteDiario - sumaRetirosHoy) <= 0)
            {
                return BadRequest("Cupo diario Excedido");
            }

            //Guardar 

            var MovimientoNew = new Movimiento
            {
                Valor = movimientoDto.Valor,
                Saldo = saldoDisponible,
                CuentaId = cuenta.Id,
                Cuenta = cuenta
            };

            _unitOfWork.Movimientos.Add(MovimientoNew);
            var movimientos = await _unitOfWork.Complete();
            var movimientosDto = _mapper.Map<IEnumerable<MovimientoDto>>(movimientos);


            return Ok(movimientosDto);
        }

        [HttpPut]
        public async Task<ActionResult<MovimientoDto>> UpdateMovimiento(MovimientoDto movimientoDtoUpdate)
        {
            if (movimientoDtoUpdate == null)
            {
                return BadRequest("No Movimiento to add");
            }
            var movimiento = await _unitOfWork.Movimientos.Find(x => x.Id == movimientoDtoUpdate.Id);
            if (movimiento == null)
            {
                return BadRequest("Movimiento not in database");
            }
            var movimientodb = movimiento;

            movimientodb.Valor = movimientoDtoUpdate.Valor;
            movimientodb.Saldo = movimientoDtoUpdate.Saldo;
            movimientodb.CuentaId = movimientoDtoUpdate.CuentaId;

            _unitOfWork.Movimientos.Update(movimientodb);
            await _unitOfWork.Complete();
            var movimientoDto = _mapper.Map<MovimientoDto>(movimientodb);

            return Ok(movimientoDto);
        }

        [HttpDelete]
        public async Task<ActionResult<List<MovimientoDto>>> RemoveMovimiento(MovimientoDto movimientoDto)
        {
            var movimientodb = await _unitOfWork.Movimientos.Find(x => x.Id == movimientoDto.Id);
            if (movimientodb == null)
            {
                return BadRequest("Movimiento not found");
            }
            _unitOfWork.Movimientos.Delete(movimientodb.Id);
            await _unitOfWork.Complete();

            var movimientos = await _unitOfWork.Movimientos.GetAll();
            var movimientosDto = _mapper.Map<IEnumerable<MovimientoDto>>(movimientos);

            return Ok(movimientosDto);
        }
    }
}
