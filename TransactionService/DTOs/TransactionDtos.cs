namespace TransactionService.DTOs
{
    public record TransactionDto(
         Guid Id,
         string? Type,
         decimal Amount,
         decimal Balance,
         Guid AccountId,
         string? AccountNumber);
    public record CreateTransactionDto(
         decimal Amount,
         Guid AccountId,
         string? AccountNumber);
    public record UpdateTransactionDto(
         Guid Id,
         string? Type,
         decimal Amount,
         decimal Balance,
         Guid AccountId,
         string? AccountNumber);
}
