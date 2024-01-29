using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace ExpedienteMedico.Models.ViewModels
{
    public class MedicineVM
    {
        [ValidateNever]
        public string HistoryId { get; set; }
        public Medicine Medicine { get; set; }

    }
}
