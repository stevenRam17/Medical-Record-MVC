using System.ComponentModel.DataAnnotations;

namespace ExpedienteMedico.Models.IntermediateTables
{
    public class MedicalHistory_Treatment
    {

        [Required]
        public string MedicalHistoryId { get; set; }


        [Required]
        public int TreatmentId { get; set; }

        public Treatment Treatments { get; set; }

        [Required]
        public int PhysicianId { get; set; }

        public Physician Physicians { get; set; }



    }
}
