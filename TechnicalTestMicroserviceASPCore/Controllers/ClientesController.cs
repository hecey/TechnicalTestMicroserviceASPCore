using Microsoft.AspNetCore.Mvc;
using TechnicalTestMicroserviceASPCore.DTOs;
using TechnicalTestMicroserviceASPCore.Models;
using TechnicalTestMicroserviceASPCore.Repositories;

namespace TechnicalTestMicroserviceASPCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : Controller
    {
        private IClienteRepository _clienteRepository;


        public ClientesController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;

        }




        [HttpGet]
        public async Task<ActionResult<List<ClienteDto>>> Get()
        {
            var clientes = await _clienteRepository.GetAll();

            return Ok(clientes);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> FindCliente(int id)
        {

            var cliente = await _clienteRepository.Get(id);
            if (cliente == null)
            {
                return BadRequest("cliente not found");
            }
            return Ok(cliente);

        }

        [HttpPost]
        public async Task<ActionResult<List<Cliente>>> Addcliente(ClienteDto clienteDto)
        {
            if (clienteDto is null)
            {
                return BadRequest("No cliente to add");
            }

            if (clienteDto.Identificacion is null)
            {
                return BadRequest("No hay identificacion de cliente to add");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object");
            }

            var clientes = await _clienteRepository.Find(x => x.Identificacion == clienteDto.Identificacion);


            if (clientes.Any())
            {
                return BadRequest("Already in database");
            }


            var clienteNuevo = new Cliente
            {
                Id = clienteDto.Id,
                Nombre = clienteDto.Nombre,
                Genero = clienteDto.Genero,
                Edad = clienteDto.Edad,
                Identificacion = clienteDto.Identificacion,
                Direccion = clienteDto.Direccion,
                Telefono = clienteDto.Telefono,
                Contrasena = clienteDto.Contrasena,
                Estado = clienteDto.Estado
            };

            _clienteRepository.Add(clienteNuevo);
            await _clienteRepository.Save();

            return Ok(await _clienteRepository.GetAll());
        }

        [HttpPut]
        public async Task<ActionResult<Cliente>> Updatecliente(ClienteDto clienteDtoUpdate)
        {
            if (clienteDtoUpdate == null)
            {
                return BadRequest("No cliente to add");
            }

            if (clienteDtoUpdate.Identificacion == null)
            {
                return BadRequest("No hay identificacion de cliente");
            }

            var clientes = await _clienteRepository.Find(x => x.Identificacion == clienteDtoUpdate.Identificacion);


            if (!clientes.Any())
            {
                return BadRequest("cliente not in database");
            }

            var clientedb = clientes.First();


            clientedb.Id = clienteDtoUpdate.Id;
            clientedb.Nombre = clienteDtoUpdate.Nombre;
            clientedb.Genero = clienteDtoUpdate.Genero;
            clientedb.Edad = clienteDtoUpdate.Edad;
            clientedb.Identificacion = clienteDtoUpdate.Identificacion;
            clientedb.Direccion = clienteDtoUpdate.Direccion;
            clientedb.Telefono = clienteDtoUpdate.Nombre;
            clientedb.Contrasena = clienteDtoUpdate.Contrasena;
            clientedb.Estado = clienteDtoUpdate.Estado;

            _clienteRepository.Update(clientedb);
            await _clienteRepository.Save();

            return Ok(clientedb);
        }
        [HttpDelete]
        public async Task<ActionResult<List<Cliente>>> Removecliente(Cliente cliente)
        {
            if (cliente.Identificacion is null)
            {
                return BadRequest("No hay identificacion de cliente");
            }

            var clientes = await _clienteRepository.Find(x => x.Identificacion == cliente.Identificacion);


            if (!clientes.Any())
            {
                return BadRequest("cliente not found");
            }
            var clientedb = clientes.First();


            _clienteRepository.Delete(clientedb.Id);
            await _clienteRepository.Save();

            return Ok(await _clienteRepository.GetAll());
        }
    }
}
