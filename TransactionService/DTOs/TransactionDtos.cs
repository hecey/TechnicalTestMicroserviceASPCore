namespace TransactionService.DTOs
{
    public record TransactionDto(
         Guid Id,
         string? Type,
         decimal Amount,
         decimal Balance,
         string? AccountNumber);
    public record CreateTransactionDto(
         decimal Amount,
         string? AccountNumber);
    public record UpdateTransactionDto(
         Guid Id,
         decimal Amount,
         decimal Balance,
         Guid AccountId,
         string? AccountNumber);
}
