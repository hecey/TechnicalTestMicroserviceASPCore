namespace AccountService.DTOs
{
    public record AccountDto(
        Guid Id,
        string? Number,
        string? Type,
        decimal InitialBalance,
        bool Status,
        string? ClientIdentification,
        string ClientName);
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
        bool Status);
}
