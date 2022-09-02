using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TTM.Api.DTOs;
using TTM.Api.Models;
using TTM.Api.UnitOfWork;

namespace TTM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CuentasController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<List<CuentaDto>>> Get()
        {
            var cuentas = await _unitOfWork.Cuentas.GetAll();
            var cuentasDto = _mapper.Map<IEnumerable<CuentaDto>>(cuentas);
            return cuentasDto.Any() ? Ok(cuentasDto) : NoContent();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CuentaDto>> FindCuenta(int id)
        {

            var cuenta = await _unitOfWork.Cuentas.Get(id);
            if (cuenta == null)
            {
                return BadRequest("Cuenta not found");
            }
            var cuentaDto = _mapper.Map<CuentaDto>(cuenta);

            return Ok(cuentaDto);

        }

        [HttpPost]
        public async Task<ActionResult<List<CuentaDto>>> AddCuenta(CuentaDto cuentaDto)
        {
            if (cuentaDto is null)
            {
                return BadRequest("No Cuenta to add");
            }


            var Cuenta = await _unitOfWork.Cuentas.Find(x => x.NumeroDeCuenta == cuentaDto.NumeroDeCuenta);
            if (Cuenta is not null)
            {
                return BadRequest("Already in database");
            }

            var cliente = await _unitOfWork.Clientes.Get(cuentaDto.ClienteId);
            if (cliente is null)
            {
                return BadRequest("Cliente not found in database");
            }

            var cuentaNueva = new Cuenta
            {
                ClienteId = cuentaDto.ClienteId,
                Estado = cuentaDto.Estado,
                NumeroDeCuenta = cuentaDto.NumeroDeCuenta,
                SaldoInicial = cuentaDto.SaldoInicial,
                TipoDeCuenta = cuentaDto.TipoDeCuenta,
                Cliente = cliente
            };

            _unitOfWork.Cuentas.Add(cuentaNueva);
            var cuentas = await _unitOfWork.Complete();
            var cuentasDto = _mapper.Map<IEnumerable<CuentaDto>>(cuentas);

            return Ok(cuentasDto);
        }

        [HttpPut]
        public async Task<ActionResult<CuentaDto>> UpdateCuenta(CuentaDto cuentaDtoUpdate)
        {
            if (cuentaDtoUpdate == null)
            {
                return BadRequest("No Cuenta to add");
            }
            var cuentadb = await _unitOfWork.Cuentas.Find(x => x.Id == cuentaDtoUpdate.Id);
            if (cuentadb is null)
            {
                return BadRequest("Cuenta not in database");
            }


            cuentadb.NumeroDeCuenta = cuentaDtoUpdate.NumeroDeCuenta;
            cuentadb.TipoDeCuenta = cuentaDtoUpdate.TipoDeCuenta;
            cuentadb.SaldoInicial = cuentaDtoUpdate.SaldoInicial;
            cuentadb.ClienteId = cuentaDtoUpdate.ClienteId;
            cuentadb.Estado = cuentaDtoUpdate.Estado;

            _unitOfWork.Cuentas.Update(cuentadb);
            await _unitOfWork.Complete();

            var cuentaDto = _mapper.Map<CuentaDto>(cuentadb);
            return Ok(cuentaDto);
        }
        [HttpDelete]
        public async Task<ActionResult<List<CuentaDto>>> RemoveCuenta(CuentaDto cuentaDto)
        {
            var cuentadb = await _unitOfWork.Cuentas.Find(x => x.Id == cuentaDto.Id);
            if (cuentadb is null)
            {
                return BadRequest("Cuenta not found");
            }

            _unitOfWork.Cuentas.Delete(cuentadb.Id);
            await _unitOfWork.Complete();

            var cuentas = await _unitOfWork.Cuentas.GetAll();
            var cuentasDto = _mapper.Map<IEnumerable<CuentaDto>>(cuentas);

            return Ok(cuentasDto);
        }
    }
}
