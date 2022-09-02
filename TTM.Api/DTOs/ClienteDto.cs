using System.ComponentModel.DataAnnotations;

namespace TTM.Api.DTOs
{
    public class ClienteDto
    {
        public int Id { get; set; }


        [StringLength(50, MinimumLength = 2, ErrorMessage = "Nombre  2 and 50 characters.")]
        public string? Nombre { get; set; }
        public char Genero { get; set; }
        [Range(1, 150)]
        public int Edad { get; set; }
        [Required]
        public string? Identificacion { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Contrasena { get; set; }
        public bool Estado { get; set; }

    }
}
