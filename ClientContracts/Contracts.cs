
using System;

namespace Hecey.TTM.ClientContracts
{
     public record ClientCreated(Guid Id,
        string? Name,
        string? Identification,
        bool Status);
     public record ClientUpdated(Guid Id,
        string? Name,
        string? Identification,
        bool Status);
     public record ClientDeleted(Guid Id,
        string? Name,
        string? Identification,
        bool Status);
}
