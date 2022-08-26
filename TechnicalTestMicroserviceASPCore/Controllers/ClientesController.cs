using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechnicalTestMicroserviceASPCore.DTOs;
using TechnicalTestMicroserviceASPCore.Models;
using TechnicalTestMicroserviceASPCore.UnitOfWork;

namespace TechnicalTestMicroserviceASPCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> Get()
        {

            try
            {
                var clientes = (List<Cliente>)await _unitOfWork.Clientes.GetAll();
                var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);
                return clientesDto is not null ? Ok(clientesDto) : NoContent();
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal server error: " + ex);
            }




        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> FindCliente(int id)
        {

            var cliente = await _unitOfWork.Clientes.Get(id);
            if (cliente == null)
            {
                return NotFound("cliente not found");
            }
            return Ok(cliente);

        }

        [HttpPost]
        public async Task<ActionResult<List<ClienteDto>>> Addcliente(ClienteDto clienteDto)
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

            var cliente = await _unitOfWork.Clientes.Find(x => x.Identificacion == clienteDto.Identificacion);


            if (cliente is not null)
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

            _unitOfWork.Clientes.Add(clienteNuevo);
            await _unitOfWork.Complete();

            return Ok(await _unitOfWork.Clientes.GetAll());
        }

        [HttpPut]
        public async Task<ActionResult<ClienteDto>> Updatecliente(ClienteDto clienteDtoUpdate)
        {
            if (clienteDtoUpdate == null)
            {
                return NotFound("No cliente to add");
            }

            if (clienteDtoUpdate.Identificacion == null)
            {
                return BadRequest("No hay identificacion de cliente");
            }

            var clientedb = await _unitOfWork.Clientes.Find(x => x.Identificacion == clienteDtoUpdate.Identificacion);


            if (clientedb is null)
            {
                return BadRequest("cliente not in database");
            }



            clientedb.Id = clienteDtoUpdate.Id;
            clientedb.Nombre = clienteDtoUpdate.Nombre;
            clientedb.Genero = clienteDtoUpdate.Genero;
            clientedb.Edad = clienteDtoUpdate.Edad;
            clientedb.Identificacion = clienteDtoUpdate.Identificacion;
            clientedb.Direccion = clienteDtoUpdate.Direccion;
            clientedb.Telefono = clienteDtoUpdate.Nombre;
            clientedb.Contrasena = clienteDtoUpdate.Contrasena;
            clientedb.Estado = clienteDtoUpdate.Estado;

            _unitOfWork.Clientes.Update(clientedb);
            await _unitOfWork.Complete();

            return Ok(clientedb);
        }
        [HttpDelete]
        public async Task<ActionResult<List<ClienteDto>>> Removecliente(Cliente cliente)
        {
            if (cliente.Identificacion is null)
            {
                return BadRequest("No hay identificacion de cliente");
            }

            var clientedb = await _unitOfWork.Clientes.Find(x => x.Identificacion == cliente.Identificacion);


            if (clientedb is null)
            {
                return NotFound("cliente not found");
            }



            _unitOfWork.Clientes.Delete(clientedb.Id);
            await _unitOfWork.Complete();

            return Ok(await _unitOfWork.Clientes.GetAll());
        }
    }
}
