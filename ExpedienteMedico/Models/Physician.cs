using System.ComponentModel.DataAnnotations;
using ExpedienteMedico.Models.IntermediateTables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ExpedienteMedico.Models
{
    public class Physician
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "College number")]
        public int CollegeNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [ValidateNever]
        [Display(Name = "Phone number")]
        public int PhoneNumber { get; set; }

        [ValidateNever]
        public string PicturePath { get; set; }

        [ValidateNever]
        public string UserId { get; set; }

        [ValidateNever]
        public User User { get; set; }


        [ValidateNever]
        public ICollection<PhysicianSpecialty> PhysicianSpecialties { get; set; }

    }
}
