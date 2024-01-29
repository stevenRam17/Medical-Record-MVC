using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace ExpedienteMedico.Models.ViewModels
{
    public class SufferingVM
    {
        [ValidateNever]
        public string HistoryId { get; set; }
        public Suffering Suffering { get; set; }

    }
}
