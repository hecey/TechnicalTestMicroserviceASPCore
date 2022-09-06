using AccountService.Clients;
using AccountService.DTOs;
using AutoMapper;
using Common.Entities;
using Common.Repositories;
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
        public async Task<ActionResult<List<AccountDto>>> Get()
        {
            var Accounts = await _repository.GetAll();
            var AccountsDto = _mapper.Map<IEnumerable<AccountDto>>(Accounts);
            return AccountsDto.Any() ? Ok(AccountsDto) : NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> FindAccount(String id)
        {
            var Account = await _repository.Find(x => x.Number == id);
            if (Account == null)
            {
                return BadRequest("Account not found");
            }
            var AccountDto = _mapper.Map<AccountDto>(Account);

            return Ok(AccountDto);
        }

        [HttpPost]
        public async Task<ActionResult<List<AccountDto>>> AddAccount(AccountDto AccountDto)
        {
            if (AccountDto is null)
            {
                return BadRequest("No Account to add");
            }


            var Account = await _repository.Find(x => x.Number == AccountDto.Number);
            if (Account is not null)
            {
                return BadRequest("Already in database");
            }

            var client = await _remoteClientService.GetClientByIdAsync(AccountDto.ClientId);

            if (client is null)
            {
                return BadRequest("Client not found in database");
            }

            var newAccount = new Account
            {
                ClientId = AccountDto.ClientId,
                Status = AccountDto.Status,
                Number = AccountDto.Number,
                InitialBalance = AccountDto.InitialBalance,
                Type = AccountDto.Type,
            };

            _repository.Add(newAccount);
            await _repository.Save();

            var Accounts = await _repository.GetAll();
            var AccountsDto = _mapper.Map<IEnumerable<AccountDto>>(Accounts);

            return Ok(AccountsDto);
        }

        [HttpPut]
        public async Task<ActionResult<AccountDto>> UpdateAccount(AccountDto AccountDtoUpdate)
        {
            if (AccountDtoUpdate == null)
            {
                return BadRequest("No Account to add");
            }
            var AccountInDB = await _repository.Find(x => x.Id == AccountDtoUpdate.Id);
            if (AccountInDB is null)
            {
                return BadRequest("Account not in database");
            }


            AccountInDB.Number = AccountDtoUpdate.Number;
            AccountInDB.Type = AccountDtoUpdate.Type;
            AccountInDB.InitialBalance = AccountDtoUpdate.InitialBalance;
            AccountInDB.ClientId = AccountDtoUpdate.ClientId;
            AccountInDB.Status = AccountDtoUpdate.Status;

            _repository.Update(AccountInDB);
            await _repository.Save();

            var AccountDto = _mapper.Map<AccountDto>(AccountInDB);
            return Ok(AccountDto);
        }
        [HttpDelete]
        public async Task<ActionResult<List<AccountDto>>> RemoveAccount(AccountDto account)
        {
            var AccountInDB = await _repository.Find(x => x.Id == account.Id);
            if (AccountInDB is null)
            {
                return BadRequest("Account not found");
            }

            _repository.Delete(AccountInDB.Id);
            await _repository.Save();

            var Accounts = await _repository.GetAll();
            var AccountsDto = _mapper.Map<IEnumerable<AccountDto>>(Accounts);

            return Ok(AccountsDto);
        }
    }
}
