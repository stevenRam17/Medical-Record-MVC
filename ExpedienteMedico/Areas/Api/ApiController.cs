using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;
using ExpedienteMedico.Models.ViewModels;
using ExpedienteMedico.Repository.IRepository;
using ExpedienteMedico.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpedienteMedico.Areas.Api
{

    [AllowAnonymous]
    [EnableCors("GeneralPolicy")]
    [Route("Api/[controller]")]
    public class ApiController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostEnvironment;
        private UserManager<IdentityUser> _userManager;

        public ApiController(IUnitOfWork unitOfWork,
            IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        //PADECIMIENTOS
        [Route("sufferings")]
        [HttpGet]
        public IActionResult GetSufferings(string id)//User id or expedient id, is the same
        {
            MedicalHistory medicalHistory = _unitOfWork.MedicalHistory.GetFirstOrDefault(x => x.UserId == id, null,
                includeProperties: "MedicalHistorySufferings");

            List<Suffering> sufferings = new List<Suffering>();

            for (int j = 0; j < medicalHistory.MedicalHistorySufferings.Count(); j++)
            {
                var aux = medicalHistory.MedicalHistorySufferings.ElementAt(j);
                var suffering = _unitOfWork.HistorySuffering.GetFirstOrDefault(x => x.SufferingId == aux.SufferingId,
                    z => z.MedicalHistoryId == aux.MedicalHistoryId, includeProperties: "Sufferings").Sufferings;
                sufferings.Add(suffering);
            }
            return Json(new { data = sufferings, success = true });
        }

        //TRATAMIENTOS
        [Route("treatments")]
        [HttpGet]
        public IActionResult GetTreatments(string id) //User id
        {
            MedicalHistory medicalHistory = _unitOfWork.MedicalHistory.GetFirstOrDefault(x => x.UserId == id, null,
                includeProperties: "MedicalHistoryTreatments");


            List<Treatment> treatments = new List<Treatment>();

            for (int j = 0; j < medicalHistory.MedicalHistoryTreatments.Count(); j++)
            {
                var aux = medicalHistory.MedicalHistoryTreatments.ElementAt(j);
                var treatment = _unitOfWork.HistoryTreatment.GetFirstOrDefault(x => x.TreatmentId == aux.TreatmentId,
                    z => z.MedicalHistoryId == aux.MedicalHistoryId, includeProperties: "Treatments").Treatments;
                treatments.Add(treatment);
            }

            return Json(new { data = treatments, success = true });
        }

        //MEDICINAS
        [Route("meds")]
        [HttpGet]
        public IActionResult GetMedicines(string id)
        {
            MedicalHistory medicalHistory = _unitOfWork.MedicalHistory.GetFirstOrDefault(x => x.UserId == id, null,
                includeProperties: "MedicalHistoryMedicines");

            List<Medicine> medicines = new List<Medicine>();

            for (int j = 0; j < medicalHistory.MedicalHistoryMedicines.Count(); j++)
            {
                var aux = medicalHistory.MedicalHistoryMedicines.ElementAt(j);
                var medicine = _unitOfWork.HistoryMedicine.GetFirstOrDefault(x => x.MedicineId == aux.MedicineId,
                    z => z.MedicalHistoryId == aux.MedicalHistoryId, includeProperties: "Medicines").Medicines;
                medicines.Add(medicine);
            }
            return Json(new { data = medicines, success = true });
        }

        //NOTAS MEDICAS
        [Route("notes")]
        [HttpGet]
        public IActionResult GetMedicalNotes(string id)
        {
            MedicalHistory medicalHistory = _unitOfWork.MedicalHistory.GetFirstOrDefault(x => x.UserId == id, null,
                includeProperties: "MedicalNotes");

            List<MedicalNote> notes = new List<MedicalNote>();

            for (int j = 0; j < medicalHistory.MedicalNotes.Count(); j++)
            {
                var aux = medicalHistory.MedicalNotes.ElementAt(j);
                notes.Add(aux);
            }

            return Json(new { data = notes, success = true });
        }

        //HISTORIAL MEDICO COMPLE
        [Route("history")]
        [HttpGet]
        public IActionResult GetHistory(string? id)
        {
            MedicalHistory medicalHistory = _unitOfWork.MedicalHistory.GetFirstOrDefault(x => x.UserId == id, null,
                includeProperties: "User,MedicalHistoryTreatments,MedicalHistorySufferings,MedicalHistoryMedicines,MedicalImages,MedicalNotes");

            if (medicalHistory == null)
            {
                medicalHistory = new MedicalHistory(id);
                _unitOfWork.MedicalHistory.Add(medicalHistory);
                _unitOfWork.Save();
            }

            //Añadiendo los objetos de datos del expediente medico del paciente

            if (medicalHistory.MedicalHistoryTreatments != null)
            {
                for (int j = 0; j < medicalHistory.MedicalHistoryTreatments.Count(); j++)
                {
                    var aux = medicalHistory.MedicalHistoryTreatments.ElementAt(j);
                    var physicianSpecialty = _unitOfWork.HistoryTreatment.GetFirstOrDefault(u => u.MedicalHistoryId == aux.MedicalHistoryId, x => x.TreatmentId == aux.TreatmentId, includeProperties: "Treatments");
                }
            }

            if (medicalHistory.MedicalHistorySufferings != null)
            {
                for (int j = 0; j < medicalHistory.MedicalHistorySufferings.Count(); j++)
                {
                    var aux = medicalHistory.MedicalHistorySufferings.ElementAt(j);
                    var physicianSpecialty = _unitOfWork.HistorySuffering.GetFirstOrDefault(u => u.MedicalHistoryId == aux.MedicalHistoryId, x => x.SufferingId == aux.SufferingId, includeProperties: "Sufferings");
                }
            }

            if (medicalHistory.MedicalHistoryMedicines != null)
            {
                for (int j = 0; j < medicalHistory.MedicalHistoryMedicines.Count(); j++)
                {
                    var aux = medicalHistory.MedicalHistoryMedicines.ElementAt(j);
                    var physicianSpecialty = _unitOfWork.HistoryMedicine.GetFirstOrDefault(u => u.MedicalHistoryId == aux.MedicalHistoryId, x => x.MedicineId == aux.MedicineId, includeProperties: "Medicines");
                }
            }

            return Json(new { data = medicalHistory, success = true });
        }

        [Route("labimages")]
        [HttpGet]
        public IActionResult GetImages(string id)
        {
            MedicalHistory medicalHistory = _unitOfWork.MedicalHistory.GetFirstOrDefault(x => x.UserId == id, null,
                includeProperties: "MedicalImages");

            List<MedicalImage> images = new List<MedicalImage>();

            for (int j = 0; j < medicalHistory.MedicalImages.Count(); j++)
            {
                var aux = medicalHistory.MedicalImages.ElementAt(j);
                images.Add(aux);
            }

            return Json(new { data = images, success = true });
        }


    }
}
