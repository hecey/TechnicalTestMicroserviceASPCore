namespace TransactionService.DTOs
{
    public record ReportDto(
         Guid Id,
         string? Type,
         decimal Amount,
         decimal Balance,
         DateTime Date,
         string? AccountNumber,
         string ClientName,
         decimal InitialBalance,
         bool AccountStatus,
         string? AccountType
         );

}

