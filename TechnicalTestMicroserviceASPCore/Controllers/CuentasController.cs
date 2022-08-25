using Microsoft.AspNetCore.Mvc;
using TechnicalTestMicroserviceASPCore.DTOs;
using TechnicalTestMicroserviceASPCore.Models;
using TechnicalTestMicroserviceASPCore.UnitOfWork;

namespace TechnicalTestMicroserviceASPCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public CuentasController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        [HttpGet]
        public async Task<ActionResult<List<CuentaDto>>> Get()
        {
            var Cuentas = await _unitOfWork.Cuentas.GetAll();

            return Ok(Cuentas);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CuentaDto>> FindCuenta(int id)
        {

            var Cuenta = await _unitOfWork.Cuentas.Get(id);
            if (Cuenta == null)
            {
                return BadRequest("Cuenta not found");
            }
            return Ok(Cuenta);

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
            await _unitOfWork.Complete();

            return Ok(await _unitOfWork.Cuentas.GetAll());
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
            return Ok(cuentadb);
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
            return Ok(await _unitOfWork.Cuentas.GetAll());
        }
    }
}
