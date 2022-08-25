using Microsoft.AspNetCore.Mvc;
using TechnicalTestMicroserviceASPCore.DTOs;
using TechnicalTestMicroserviceASPCore.Models;
using TechnicalTestMicroserviceASPCore.Repositories;

namespace TechnicalTestMicroserviceASPCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : Controller
    {
        private ICuentaRepository _cuentaRepository;
        private IClienteRepository _clienteRepository;


        public CuentasController(ICuentaRepository cuentaRepository, IClienteRepository clienteRepository)
        {
            _cuentaRepository = cuentaRepository;
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<CuentaDto>>> Get()
        {
            var Cuentas = await _cuentaRepository.GetAll();

            return Ok(Cuentas);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CuentaDto>> FindCuenta(int id)
        {

            var Cuenta = await _cuentaRepository.Get(id);
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


            var Cuenta = await _cuentaRepository.Find(x => x.NumeroDeCuenta == cuentaDto.NumeroDeCuenta);
            if (Cuenta is not null)
            {
                return BadRequest("Already in database");
            }

            var cliente = await _clienteRepository.Get(cuentaDto.ClienteId);
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

            _cuentaRepository.Add(cuentaNueva);
            await _cuentaRepository.Save();

            return Ok(await _cuentaRepository.GetAll());
        }

        [HttpPut]
        public async Task<ActionResult<CuentaDto>> UpdateCuenta(CuentaDto cuentaDtoUpdate)
        {
            if (cuentaDtoUpdate == null)
            {
                return BadRequest("No Cuenta to add");
            }
            var cuentadb = await _cuentaRepository.Find(x => x.Id == cuentaDtoUpdate.Id);
            if (cuentadb is null)
            {
                return BadRequest("Cuenta not in database");
            }


            cuentadb.NumeroDeCuenta = cuentaDtoUpdate.NumeroDeCuenta;
            cuentadb.TipoDeCuenta = cuentaDtoUpdate.TipoDeCuenta;
            cuentadb.SaldoInicial = cuentaDtoUpdate.SaldoInicial;
            cuentadb.ClienteId = cuentaDtoUpdate.ClienteId;
            cuentadb.Estado = cuentaDtoUpdate.Estado;

            _cuentaRepository.Update(cuentadb);

            await _cuentaRepository.Save();
            return Ok(cuentadb);
        }
        [HttpDelete]
        public async Task<ActionResult<List<CuentaDto>>> RemoveCuenta(CuentaDto cuentaDto)
        {
            var cuentadb = await _cuentaRepository.Find(x => x.Id == cuentaDto.Id);
            if (cuentadb is null)
            {
                return BadRequest("Cuenta not found");
            }

            _cuentaRepository.Delete(cuentadb.Id);
            await _cuentaRepository.Save();
            return Ok(await _cuentaRepository.GetAll());
        }
    }
}
