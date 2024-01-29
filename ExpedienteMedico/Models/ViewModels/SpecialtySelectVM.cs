using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ExpedienteMedico.Models.ViewModels
{
    public class SpecialtySelectVM
    {

        [ValidateNever]
        public int SpecialtyId { get; set; }

        [ValidateNever]
        public string Name { get; set; }

        [ValidateNever]
        public bool IsSelected { get; set; }

    }
}
