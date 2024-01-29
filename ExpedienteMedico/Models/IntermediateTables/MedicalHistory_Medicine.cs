using System.ComponentModel.DataAnnotations;

namespace ExpedienteMedico.Models.IntermediateTables
{
    public class MedicalHistory_Medicine
    {

        [Required]
        public string MedicalHistoryId { get; set; }
        

        [Required]
        public int MedicineId { get; set; }

        public Medicine Medicines { get; set; }

        [Required]
        public int PhysicianId { get; set; }

        public Physician Physicians { get; set; }

    }
}
