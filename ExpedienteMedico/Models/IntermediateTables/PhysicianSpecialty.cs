using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ExpedienteMedico.Models.IntermediateTables
{

    public class PhysicianSpecialty
    {
        [Required]
        public int PhysicianId { get; set; }

        [Required]
        public int SpecialtyId { get; set; }
        
        public Specialty Specialty { get; set; }

    }
}
