using AutoMapper;
using TransactionService.Entities;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Clients;
using TransactionService.DTOs;
using TransactionService.Repositories;

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
            var transaction = await _repository.GetAsync(id);
            if (transaction == null)
            {
                return NotFound("Transaction not found");
            }
            var transactionDto = _mapper.Map<TransactionDto>(transaction);

            return Ok(transactionDto);
        }

        [HttpGet("ReportByClient/{id}/{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<ReportDto>>> ReportByIDAndRangeDate(string id, DateTime startDate, DateTime endDate)
        {
            var transactions = await _repository.ReportByIDAndRangeDate(id, startDate, endDate);

            if (!transactions.Any())
            {
                return BadRequest("Transactions not found in database");
            }

            return Ok(transactions);
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
            if (createTransactionDto.AccountNumber is null)
            {
                return BadRequest("No AccountNumber");
            }

            var accountDto = await _remoteAccountService.GetAccountByNumberAsync(createTransactionDto.AccountNumber);

            if (accountDto == null)
            {
                return BadRequest("Account not found in database");
            }

            var lastTransaction = await _repository.LastTransactionByAccount(accountDto.Number!);

            decimal lastBalance = lastTransaction is not null ?
                  lastTransaction.Balance :
                  accountDto.InitialBalance;
            if (createTransactionDto.Amount < 0)
            {
                if (lastBalance + createTransactionDto.Amount < 0)
                {
                    return BadRequest("Amount no available for withdrawn");
                }
            }

            // Los valores cuando son crédito son positivos, y los débitos son negativos.
            // Debe almacenarse el saldo disponible en cada transacción dependiendo del
            // tipo de Transaction.

            // Si el saldo es menor a la transacción débito, debe desplegar mensaje
            // “Saldo no disponible”
            decimal balanceAvailable = lastBalance + createTransactionDto.Amount;

            // Se debe tener un parámetro de limite diario de retiro (valor tope 1000$)
            var dailyLimitForWithdrawn = _configRoot.GetValue<decimal>("dailyLimitForWithdrawn");

            if (dailyLimitForWithdrawn <= 0)
            {
                return BadRequest("No se ha definido limite diario en el sistema");
            }

            var amountOfTodayWithdrawn = await _repository.FindTodaysBalanceUsed(accountDto.Number!);

            if ((dailyLimitForWithdrawn - amountOfTodayWithdrawn) <= 0)
            {
                return BadRequest("Cupo diario Excedido");
            }

            //Save
            var newTransaction = new Transaction
            {
                Amount = createTransactionDto.Amount,
                Balance = balanceAvailable,
                AccountNumber = createTransactionDto.AccountNumber
            };

            _repository.Add(newTransaction);
            await _repository.SaveAsync();

            var transactionDto = _mapper.Map<TransactionDto>(newTransaction);

            return Ok(transactionDto);
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

            _repository.Update(transactionDB);
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
            _repository.Delete(transactionDB.Id);
            await _repository.SaveAsync();

            return Ok(transactionDB);
        }
    }
}
