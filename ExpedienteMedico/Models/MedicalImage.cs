using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ExpedienteMedico.Models
{
    public class MedicalImage
    {
        [Key]
        public int Id { get; set; }
        
        [ValidateNever]
        public string ImageUrl { get; set; }

        [ValidateNever]
        public string PdfUrl { get; set; }
        
        [Required]
        public string MedicalHistoryId { get; set; }

        [Required]
        public int PhysicianId { get; set; }

        [ValidateNever]
        public Physician Physician { get; set; }

        [StringLength(200)]
        [Required]
        public string Description { get; set; }

    }
}
