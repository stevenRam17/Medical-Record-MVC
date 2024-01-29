using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace ExpedienteMedico.Models.ViewModels
{
    public class PhysicianCreateVM
    {
        public Physician Physician { get; set; }

        [ValidateNever]
        public List<SpecialtySelectVM> Specialties { get; set; }

    }
}
