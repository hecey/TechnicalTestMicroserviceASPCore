using AutoMapper;
using ClientService.DTOs;
using Common.Entities;
using Common.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository<Client> _repository;
        private readonly IMapper _mapper;
        public ClientController(IClientRepository<Client> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetAsync()
        {
            try
            {
                var clients = (List<Client>)await _repository.GetAll();
                var clientsDto = _mapper.Map<IEnumerable<ClientDto>>(clients);
                return clientsDto.Any() ? Ok(clientsDto) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> FindAsync(Guid id)
        {

            var client = await _repository.Get(id);
            if (client == null)
            {
                return NotFound("client not found");
            }
            var clientDto = _mapper.Map<ClientDto>(client);

            return Ok(clientDto);

        }

        [HttpPost]
        public async Task<ActionResult<List<ClientDto>>> AddAsync(ClientDto clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (clientDto.Identification is null)
            {
                return BadRequest("No hay identification de client to add");
            }


            var client = await _repository.Find(x => x.Identification == clientDto.Identification);


            if (client is not null)
            {
                return BadRequest("Already in database");
            }


            var newClient = new Client
            {
                Id = clientDto.Id,
                Name = clientDto.Name,
                Genre = clientDto.Genre,
                Age = clientDto.Age,
                Identification = clientDto.Identification,
                Address = clientDto.Address,
                Phone = clientDto.Phone,
                Password = clientDto.Password,
                Status = clientDto.Status
            };

            _repository.Add(newClient);
            await _repository.Save();

            var clients = await _repository.GetAll();
            var clientsDto = _mapper.Map<IEnumerable<ClientDto>>(clients);

            return Ok(clientsDto);
        }

        [HttpPut]
        public async Task<ActionResult<ClientDto>> UpdateAsync(ClientDto clientDtoUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (clientDtoUpdate.Identification == null)
            {
                return BadRequest("No hay identification de client");
            }

            var clientDB = await _repository.Find(x => x.Identification == clientDtoUpdate.Identification);


            if (clientDB is null)
            {
                return BadRequest("client not in database");
            }



            clientDB.Id = clientDtoUpdate.Id;
            clientDB.Name = clientDtoUpdate.Name;
            clientDB.Genre = clientDtoUpdate.Genre;
            clientDB.Age = clientDtoUpdate.Age;
            clientDB.Identification = clientDtoUpdate.Identification;
            clientDB.Address = clientDtoUpdate.Address;
            clientDB.Phone = clientDtoUpdate.Name;
            clientDB.Password = clientDtoUpdate.Password;
            clientDB.Status = clientDtoUpdate.Status;

            _repository.Update(clientDB);
            await _repository.Save();

            var clientDto = _mapper.Map<ClientDto>(clientDB);
            return Ok(clientDto);
        }
        [HttpDelete]
        public async Task<ActionResult<List<ClientDto>>> RemoveAsync(ClientDto clientDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (clientDto.Identification is null)
            {
                return BadRequest("No hay identification de client");
            }

            var clientDB = await _repository.Find(x => x.Identification == clientDto.Identification);


            if (clientDB is null)
            {
                return NotFound("client not found");
            }



            _repository.Delete(clientDB.Id);
            await _repository.Save();

            var clients = await _repository.GetAll();
            var clientsDto = _mapper.Map<IEnumerable<ClientDto>>(clients);

            return Ok(clientsDto);
        }
    }
}
