using AutoMapper;
using Common.Entities;
using Common.Repositories;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Clients;
using TransactionService.DTOs;

namespace TransactionService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly RemoteAccountService _remoteAccountService;
        private readonly IConfiguration _configRoot;
        private readonly ITransactionRepository<Transaction> _repository;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionRepository<Transaction> repository, IConfiguration configRoot, IMapper mapper, RemoteAccountService remoteAccountService)
        {
            _repository = repository;
            _configRoot = (IConfigurationRoot)configRoot;
            _mapper = mapper;
            _remoteAccountService = remoteAccountService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionDto>>> GetAsync()
        {
            var Transactions = await _repository.GetAsync();
            var TransactionsDto = _mapper.Map<IEnumerable<TransactionDto>>(Transactions);

            return TransactionsDto.Any() ? Ok(TransactionsDto) : NoContent();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetByIdAsync(Guid id)
        {
            var Transaction = await _repository.GetAsync(id);
            if (Transaction == null)
            {
                return NotFound("Transaction not found");
            }
            var TransactionDto = _mapper.Map<TransactionDto>(Transaction);

            return Ok(TransactionDto);
        }

        [HttpPost]
        public async Task<ActionResult<List<TransactionDto>>> PostAsync(CreateTransactionDto createTransactionDto)
        {
            if (createTransactionDto is null)
            {
                return BadRequest("No Transaction to add");
            }

            if (createTransactionDto.Amount == 0)
            {
                return BadRequest("Valor es 0");
            }

            var accountDto = await _remoteAccountService.GetAccountByIdAsync(createTransactionDto.AccountNumber!);

            if (accountDto == null)
            {
                return BadRequest("Account not found in database");
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

            if (createTransactionDto.Amount < 0)
            {
                if (lastBalance + createTransactionDto.Amount < 0)
                {
                    return BadRequest("Amount no available for withdrawn");
                }
            }

            balanceAvailable = lastBalance + createTransactionDto.Amount;

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
                Amount = createTransactionDto.Amount,
                Balance = balanceAvailable,
                AccountId = accountDto.Id,
                Account = account,
            };

            _repository.AddAsync(newTransaction);
            await _repository.SaveAsync();


            var transactionDto = _mapper.Map<TransactionDto>(newTransaction);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = newTransaction.Id }, transactionDto);

        }

        [HttpPut]
        public async Task<ActionResult<TransactionDto>> PutAsync(UpdateTransactionDto updateTransactionDto)
        {
            if (updateTransactionDto == null)
            {
                return BadRequest("No Transaction to add");
            }
            var transactionDB = await _repository.FindAsync(x => x.Id == updateTransactionDto.Id);
            if (transactionDB == null)
            {
                return NotFound("Transaction not in database");
            }

            transactionDB.Amount = updateTransactionDto.Amount;
            transactionDB.Balance = updateTransactionDto.Balance;
            transactionDB.AccountId = updateTransactionDto.AccountId;

            _repository.UpdateAsync(transactionDB);
            await _repository.SaveAsync();
            var TransactionDto = _mapper.Map<TransactionDto>(transactionDB);

            return Ok(TransactionDto);
        }

        [HttpDelete]
        public async Task<ActionResult<List<TransactionDto>>> DeleteAsync(TransactionDto transactionDto)
        {
            var transactionDB = await _repository.FindAsync(x => x.Id == transactionDto.Id);
            if (transactionDB == null)
            {
                return NotFound("Transaction not found");
            }
            _repository.DeleteAsync(transactionDB.Id);
            await _repository.SaveAsync();

            return Ok(transactionDB);
        }
    }
}
