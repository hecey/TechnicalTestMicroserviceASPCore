
using System;

namespace Hecey.TTM.ClientContracts
{
     public record ClientCreated(Guid Id,
        string? Name,
        char Genre,
        int Age,
        string? Identification,
        string? Address,
        string? Phone,
        string? Password,
        bool Status);
     public record ClientUpdated(Guid Id,
        string? Name,
        char Genre,
        int Age,
        string? Identification,
        string? Address,
        string? Phone,
        string? Password,
        bool Status);
     public record ClientDeleted(Guid Id,
        string? Name,
        char Genre,
        int Age,
        string? Identification,
        string? Address,
        string? Phone,
        string? Password,
        bool Status);
}
