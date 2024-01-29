using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using ExpedienteMedico.Models.IntermediateTables;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ExpedienteMedico.Models
{
    public class MedicalHistory
    {

        public MedicalHistory(string userId)
        {
            UserId = userId;
        }


        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }

        [ValidateNever]
        public ICollection<MedicalImage> MedicalImages { get; set; }

        [ValidateNever]
        public ICollection<MedicalNote> MedicalNotes { get; set; }

        [ValidateNever]
        public ICollection<MedicalHistory_Treatment> MedicalHistoryTreatments { get; set; }

        [ValidateNever]
        public ICollection<MedicalHistory_Suffering> MedicalHistorySufferings { get; set; }

        [ValidateNever]
        public ICollection<MedicalHistory_Medicine> MedicalHistoryMedicines { get; set; }


    }
}
