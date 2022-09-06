using AutoMapper;
using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Clients;
using TransactionService.DTOs;
using TransactionService.Repositories;

namespace TransactionService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : Controller
    {
        private readonly RemoteAccountService _remoteAccountService;
        private readonly IConfiguration _configRoot;
        private readonly TransactionRepository<Transaction> _repository;
        private readonly IMapper _mapper;

        public TransactionsController(TransactionRepository<Transaction> repository, IConfiguration configRoot, IMapper mapper, RemoteAccountService remoteAccountService)
        {
            _repository = repository;
            _configRoot = (IConfigurationRoot)configRoot;
            _mapper = mapper;
            _remoteAccountService = remoteAccountService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionDto>>> Get()
        {
            var Transactions = await _repository.GetAll();
            var TransactionsDto = _mapper.Map<IEnumerable<TransactionDto>>(Transactions);

            return TransactionsDto.Any() ? Ok(TransactionsDto) : NoContent();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> Find(Guid id)
        {
            var Transaction = await _repository.Get(id);
            if (Transaction == null)
            {
                return BadRequest("Transaction not found");
            }
            var TransactionDto = _mapper.Map<TransactionDto>(Transaction);

            return Ok(TransactionDto);
        }

        [HttpPost]
        public async Task<ActionResult<List<TransactionDto>>> Add(TransactionDto transactionDto)
        {
            if (transactionDto is null)
            {
                return BadRequest("No Transaction to add");
            }

            var Transaction = await _repository.Find(x => x.Id == transactionDto.Id);
            if (Transaction != null)
            {
                return BadRequest("Already in database");
            }

            var accountDto = await _remoteAccountService.GetAccountByIdAsync(transactionDto.AccountNumber!);

            if (accountDto == null)
            {
                return BadRequest("Account not found in database");
            }

            if (transactionDto.Amount == 0)
            {
                return BadRequest("Valor es 0");
            }

            // Los valores cuando son crédito son positivos, y los débitos son negativos.
            // Debe almacenarse el saldo disponible en cada transacción dependiendo del
            // tipo de Transaction.

            // Si el saldo es menor a la transacción débito, debe desplegar mensaje
            // “Saldo no disponible”
            decimal balanceAvailable = 0;
            decimal lastBalance = 0;

            var lastTransaction = await _repository.LastTransactionByAccount(accountDto.Id);

            lastBalance = lastTransaction is not null ?
                              lastTransaction.Amount :
                              accountDto.InitialBalance;

            if (transactionDto.Amount < 0)
            {
                if (lastBalance + transactionDto.Amount < 0)
                {
                    return BadRequest("Amount no available for withdrawn");
                }
            }

            balanceAvailable = lastBalance + transactionDto.Amount;

            // Se debe tener un parámetro de limite diario de retiro (valor tope 1000$)

            var dailyLimitForWithdrawn = _configRoot.GetValue<decimal>("dailyLimitForWithdrawn");

            if (dailyLimitForWithdrawn <= 0)
            {
                return BadRequest("No se ha definido limite diario en el sistema");
            }

            var amountOfTodayWithdrawn = await _repository.FindTodaysBalanceUsed(accountDto.Id);

            if ((dailyLimitForWithdrawn - amountOfTodayWithdrawn) <= 0)
            {
                return BadRequest("Cupo diario Excedido");
            }

            //Save
            var account = _mapper.Map<Account>(accountDto);

            var newTransaction = new Transaction
            {
                Amount = transactionDto.Amount,
                Balance = balanceAvailable,
                AccountId = accountDto.Id,
                Account = account
            };

            _repository.Add(newTransaction);
            await _repository.Save();

            var Transactions = await _repository.GetAll();
            var TransactionsDto = _mapper.Map<IEnumerable<TransactionDto>>(Transactions);

            return Ok(TransactionsDto);
        }

        [HttpPut]
        public async Task<ActionResult<TransactionDto>> UpdateTransaction(TransactionDto TransactionDtoUpdate)
        {
            if (TransactionDtoUpdate == null)
            {
                return BadRequest("No Transaction to add");
            }
            var transactionDB = await _repository.Find(x => x.Id == TransactionDtoUpdate.Id);
            if (transactionDB == null)
            {
                return BadRequest("Transaction not in database");
            }

            transactionDB.Amount = TransactionDtoUpdate.Amount;
            transactionDB.Balance = TransactionDtoUpdate.Balance;
            transactionDB.AccountId = TransactionDtoUpdate.AccountId;

            _repository.Update(transactionDB);
            await _repository.Save();
            var TransactionDto = _mapper.Map<TransactionDto>(transactionDB);

            return Ok(TransactionDto);
        }

        [HttpDelete]
        public async Task<ActionResult<List<TransactionDto>>> RemoveTransaction(TransactionDto transactionDto)
        {
            var transactionDB = await _repository.Find(x => x.Id == transactionDto.Id);
            if (transactionDB == null)
            {
                return BadRequest("Transaction not found");
            }
            _repository.Delete(transactionDB.Id);
            await _repository.Save();

            var transactions = await _repository.GetAll();
            var transactionsDto = _mapper.Map<IEnumerable<TransactionDto>>(transactions);

            return Ok(transactionsDto);
        }
    }
}
