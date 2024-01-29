using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace ExpedienteMedico.Models.ViewModels
{
    public class TreatmentVM
    {
        [ValidateNever]
        public string HistoryId { get; set; }

        public Treatment Treatment { get; set; }

    }
}
