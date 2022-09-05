using System.ComponentModel.DataAnnotations;

namespace AccountService.DTOs
{
    public record AccountDto(
        Guid Id,
        string? Number,
        string? Type,
        decimal InitialBalance,
        bool Status,
        Guid ClientId);
    public record CreateAccountDto(
        Guid Id,
        string? Number,
        string? Type,
        decimal InitialBalance,
        bool Status,
        Guid ClientId);
    public record UpdateAccountDto(
        Guid Id,
        string? Number,
        string? Type,
        decimal InitialBalance,
        bool Status,
        Guid ClientId);
}
