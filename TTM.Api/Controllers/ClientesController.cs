using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TTM.Api.DTOs;
using TTM.Api.Models;
using TTM.Api.UnitOfWork;

namespace TTM.Api.Controllers
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
                return clientesDto.Any() ? Ok(clientesDto) : NoContent();
            }
            catch (Exception ex)
            {
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
            var clienteDto = _mapper.Map<ClienteDto>(cliente);

            return Ok(clienteDto);

        }

        [HttpPost]
        public async Task<ActionResult<List<ClienteDto>>> Addcliente(ClienteDto clienteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (clienteDto.Identificacion is null)
            {
                return BadRequest("No hay identificacion de cliente to add");
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

            var clientes = await _unitOfWork.Clientes.GetAll();
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);

            return Ok(clientesDto);
        }

        [HttpPut]
        public async Task<ActionResult<ClienteDto>> Updatecliente(ClienteDto clienteDtoUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

            var clienteDto = _mapper.Map<ClienteDto>(clientedb);
            return Ok(clienteDto);
        }
        [HttpDelete]
        public async Task<ActionResult<List<ClienteDto>>> Removecliente(ClienteDto clienteDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (clienteDto.Identificacion is null)
            {
                return BadRequest("No hay identificacion de cliente");
            }

            var clientedb = await _unitOfWork.Clientes.Find(x => x.Identificacion == clienteDto.Identificacion);


            if (clientedb is null)
            {
                return NotFound("cliente not found");
            }



            _unitOfWork.Clientes.Delete(clientedb.Id);
            await _unitOfWork.Complete();

            var clientes = await _unitOfWork.Clientes.GetAll();
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);

            return Ok(clientesDto);
        }
    }
}
