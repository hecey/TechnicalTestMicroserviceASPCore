using System;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.DTOs
{
    public record TransactionDto(
         Guid Id,
         string? Type ,
         decimal Amount ,
         decimal Balance,
         Guid AccountId,
         string? AccountNumber);
    public record CreateTransactionDto(
         Guid Id,
         string? Type ,
         decimal Amount ,
         decimal Balance,
         Guid AccountId,
         string? AccountNumber);
    public record UpdateTransactionDto(
         Guid Id,
         string? Type ,
         decimal Amount ,
         decimal Balance,
         Guid AccountId,
         string? AccountNumber);
}
