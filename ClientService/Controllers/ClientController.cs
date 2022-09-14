using AutoMapper;
using ClientService.DTOs;
using Hecey.TTM.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using ClientService.Repositories;

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

            var clients = (List<Client>)await _repository.GetAsync();
            var clientsDto = _mapper.Map<IEnumerable<ClientDto>>(clients);
            return clientsDto.Any() ? Ok(clientsDto) : NoContent();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetByIdAsync(string id)
        {

            var client = await _repository.FindAsync(x => x.Identification == id);
            if (client == null)
            {
                return NotFound("client not found");
            }
            var clientDto = _mapper.Map<ClientDto>(client);

            return Ok(clientDto);

        }

        [HttpPost]
        public async Task<ActionResult<List<ClientDto>>> PostAsync(CreateClientDto createClientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (createClientDto.Identification is null)
            {
                return BadRequest("No hay identification de client to add");
            }


            var client = await _repository.FindAsync(x => x.Identification == createClientDto.Identification);


            if (client is not null)
            {
                return BadRequest("Already in database");
            }


            var newClient = new Client
            {
                Id = Guid.NewGuid(),
                Name = createClientDto.Name,
                Genre = createClientDto.Genre,
                Age = createClientDto.Age,
                Identification = createClientDto.Identification,
                Address = createClientDto.Address,
                Phone = createClientDto.Phone,
                Password = createClientDto.Password,
                Status = createClientDto.Status
            };

            _repository.Add(newClient);
            await _repository.SaveAsync();

            var clientDto = _mapper.Map<ClientDto>(newClient);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = newClient.Id }, clientDto);
        }

        [HttpPut]
        public async Task<ActionResult<ClientDto>> PutAsync(UpdateClientDto updateClientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (updateClientDto.Identification == null)
            {
                return BadRequest("No hay identification de client");
            }

            var clientDB = await _repository.FindAsync(x => x.Identification == updateClientDto.Identification);


            if (clientDB is null)
            {
                return BadRequest("client not in database");
            }


            clientDB.Name = updateClientDto.Name;
            clientDB.Genre = updateClientDto.Genre;
            clientDB.Age = updateClientDto.Age;
            clientDB.Identification = updateClientDto.Identification;
            clientDB.Address = updateClientDto.Address;
            clientDB.Phone = updateClientDto.Name;
            clientDB.Password = updateClientDto.Password;
            clientDB.Status = updateClientDto.Status;

            _repository.Update(clientDB);
            await _repository.SaveAsync();

            var clientDto = _mapper.Map<ClientDto>(clientDB);
            return Ok(clientDto);
        }
        [HttpDelete]
        public async Task<ActionResult<List<ClientDto>>> DeletAsync(ClientDto clientDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (clientDto.Identification is null)
            {
                return BadRequest("No hay identification de client");
            }

            var clientDB = await _repository.FindAsync(x => x.Identification == clientDto.Identification);


            if (clientDB is null)
            {
                return NotFound("client not found");
            }



            _repository.Delete(clientDB.Id);
            await _repository.SaveAsync();

            return Ok(clientDB);
        }
    }
}
