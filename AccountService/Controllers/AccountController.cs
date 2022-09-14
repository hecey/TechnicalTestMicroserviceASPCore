using AccountService.Clients;
using AccountService.DTOs;
using AutoMapper;
using Hecey.TTM.Common.Entities;
using AccountService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly RemoteClientService _remoteClientService;
        private readonly IAccountRepository<Account> _repository;
        private readonly IMapper _mapper;
        public AccountController(IAccountRepository<Account> repository, IMapper mapper, RemoteClientService remoteClientService)
        {
            _repository = repository;
            _mapper = mapper;
            _remoteClientService = remoteClientService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountDto>>> GetAsync()
        {
            var Accounts = await _repository.GetAsync();
            var AccountsDto = _mapper.Map<IEnumerable<AccountDto>>(Accounts);
            return AccountsDto.Any() ? Ok(AccountsDto) : NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetByNumberAsync(String id)
        {
            var Account = await _repository.FindAsync(x => x.Number == id);
            if (Account == null)
            {
                return BadRequest("Account not found");
            }
            var AccountDto = _mapper.Map<AccountDto>(Account);

            return Ok(AccountDto);
        }


        [HttpGet("GetByClient/{id}")]
        public async Task<ActionResult<AccountDto>> GetByClientIdentificationAsync(String id)
        {
            var accounts = await _repository.FindAsync(x => x.ClientIdentification == id, null, "");
            if (accounts == null)
            {
                return BadRequest("Account not found");
            }
            var AccountsDto = _mapper.Map<IEnumerable<AccountDto>>(accounts);

            return Ok(AccountsDto);
        }

        [HttpPost]
        public async Task<ActionResult<List<AccountDto>>> PostAsync(CreateAccountDto createAccountDto)
        {
            if (createAccountDto is null)
            {
                return BadRequest("No Account to add");
            }


            var Account = await _repository.FindAsync(x => x.Number == createAccountDto.Number);
            if (Account is not null)
            {
                return BadRequest("Already in database");
            }

            var clientDto = await _remoteClientService.GetClientByIdAsync(createAccountDto.ClientIdentification);

            if (clientDto is null)
            {
                return BadRequest("Client not found in database");
            }

            var newAccount = new Account
            {
                Id = Guid.NewGuid(),
                ClientIdentification = createAccountDto.ClientIdentification,
                Status = createAccountDto.Status,
                Number = createAccountDto.Number,
                InitialBalance = createAccountDto.InitialBalance,
                Type = createAccountDto.Type,
                ClientName = clientDto.Name
            };

            _repository.Add(newAccount);
            await _repository.SaveAsync();

            var AccountDto = _mapper.Map<AccountDto>(newAccount);

            return CreatedAtAction(nameof(GetByNumberAsync), new { id = newAccount.Id }, AccountDto);
        }

        [HttpPut]
        public async Task<ActionResult<AccountDto>> PutAsync(UpdateAccountDto updateAccountDto)
        {
            if (updateAccountDto == null)
            {
                return BadRequest("No Account to add");
            }
            var AccountInDB = await _repository.FindAsync(x => x.Number == updateAccountDto.Number);
            if (AccountInDB is null)
            {
                return BadRequest("Account number not in database");
            }

            AccountInDB.Type = updateAccountDto.Type;
            AccountInDB.InitialBalance = updateAccountDto.InitialBalance;
            AccountInDB.Status = updateAccountDto.Status;

            _repository.Update(AccountInDB);
            await _repository.SaveAsync();

            var AccountDto = _mapper.Map<AccountDto>(AccountInDB);
            return Ok(AccountDto);
        }

        [HttpDelete]
        public async Task<ActionResult<List<AccountDto>>> DeleteAsync(AccountDto account)
        {
            var AccountInDB = await _repository.FindAsync(x => x.Id == account.Id);
            if (AccountInDB is null)
            {
                return NotFound("Account not found");
            }

            _repository.Delete(AccountInDB.Id);
            await _repository.SaveAsync();

            return Ok(AccountInDB);
        }
    }
}
