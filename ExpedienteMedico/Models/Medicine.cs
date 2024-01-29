using System.ComponentModel.DataAnnotations;

namespace ExpedienteMedico.Models
{
    public class Medicine
    {
        [Key]
        public int Id{ get; set; }

        [Required]
        public string Name {get; set; }

        [Required]
        public bool IsSuspended { get; set; }
    }
}
