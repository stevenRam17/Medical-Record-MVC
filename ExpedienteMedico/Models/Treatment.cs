using System.ComponentModel.DataAnnotations;

namespace ExpedienteMedico.Models
{
    public class Treatment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public bool IsSuspended { get; set; }

    }
}
