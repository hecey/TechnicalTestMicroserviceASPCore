using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TTM.Common.Interfaces;

namespace TTM.Common.Entities
{

    public class Person : IEntity
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name  2 and 50 characters.")]
        public string? Name { get; set; }
        public char Genre { get; set; }
        [Range(1, 150)]
        public int Age { get; set; }
        [Required]
        public string? Identification { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
