using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TTM.ClientService.DTOs;
using TTM.ClientService.Repositories;
using TTM.Common.Entities;
using TTM.Common.UnitOfWork;

namespace TTM.ClientService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IUnitOfWork<ClientRepository<Client>> _unitOfWork;
        private readonly IMapper _mapper;
        public ClientsController(IUnitOfWork<ClientRepository<Client>> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetAsync()
        {
            try
            {
                var clients = (List<Client>)await _unitOfWork.Repository.GetAll();
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

            var client = await _unitOfWork.Repository.Get(id);
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


            var client = await _unitOfWork.Repository.Find(x => x.Identification == clientDto.Identification);


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

            _unitOfWork.Repository.Add(newClient);
            await _unitOfWork.Complete();

            var clients = await _unitOfWork.Repository.GetAll();
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

            var clientDB = await _unitOfWork.Repository.Find(x => x.Identification == clientDtoUpdate.Identification);


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

            _unitOfWork.Repository.Update(clientDB);
            await _unitOfWork.Complete();

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

            var clientDB = await _unitOfWork.Repository.Find(x => x.Identification == clientDto.Identification);


            if (clientDB is null)
            {
                return NotFound("client not found");
            }



            _unitOfWork.Repository.Delete(clientDB.Id);
            await _unitOfWork.Complete();

            var clients = await _unitOfWork.Repository.GetAll();
            var clientsDto = _mapper.Map<IEnumerable<ClientDto>>(clients);

            return Ok(clientsDto);
        }
    }
}
