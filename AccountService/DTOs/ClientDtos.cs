using System.ComponentModel.DataAnnotations;

namespace ClientService.DTOs
{
    public record ClientDto(
        Guid Id,
        string Name,
        string Identification);
}
