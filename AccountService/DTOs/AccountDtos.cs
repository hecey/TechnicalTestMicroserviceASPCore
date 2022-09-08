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
        string? Number,
        string? Type,
        decimal InitialBalance,
        bool Status,
        string ClientIdentification);
    public record UpdateAccountDto(
        string? Number,
        string? Type,
        decimal InitialBalance,
        bool Status,
        Guid ClientId);
}
