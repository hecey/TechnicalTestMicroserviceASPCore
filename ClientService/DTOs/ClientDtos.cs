using System.ComponentModel.DataAnnotations;

namespace ClientService.DTOs
{
    public record ClientDto(
        Guid Id,
        string Name,
        char Genre,
        int Age,
        string Identification,
        string Address,
        string Phone,
        string Password,
        bool Status);
    public record CreateClientDto(
        string Name,
        char Genre,
        int Age,
        [Required] string Identification,
        string Address,
        string Phone,
        string Password,
        bool Status);
    public record UpdateClientDto(
       string Name,
       char Genre,
       int Age,
       [Required] string Identification,
       string Address,
       string Phone,
       string Password,
       bool Status);
}
