using System.ComponentModel.DataAnnotations;

namespace TransactionService.DTOs
{
    public record AccountDto(
        Guid Id,
        string? Number,
        string? Type,
        decimal InitialBalance,
        bool Status,
        Guid ClientId);
}
