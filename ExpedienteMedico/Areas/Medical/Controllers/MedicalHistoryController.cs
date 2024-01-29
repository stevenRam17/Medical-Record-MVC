
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Windows;
using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;
using ExpedienteMedico.Models.ViewModels;
using ExpedienteMedico.Repository.IRepository;
using ExpedienteMedico.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;


namespace ExpedienteMedico.Areas.Medical.Controllers
{

    [Area("Medical")]
    [Authorize(Roles = Roles.Role_Admin + "," + Roles.Role_Physician)]

    public class MedicalHistoryController : Controller
    {

        #region HTTP GET POST

        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostEnvironment;

        public MedicalHistoryController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        //GET ********************************
        public IActionResult Upsert(string? id)  //ID of user
        {

            MedicalHistory medicalHistory = _unitOfWork.MedicalHistory.GetFirstOrDefault(x => x.UserId == id, null,
                includeProperties: "User,MedicalHistoryTreatments,MedicalHistorySufferings,MedicalHistoryMedicines,MedicalImages,MedicalNotes");

            if (medicalHistory == null)
            {
                medicalHistory = new MedicalHistory(id);
                _unitOfWork.MedicalHistory.Add(medicalHistory);
                _unitOfWork.Save();
            }

            //establecer la fecha de atendido al cliente cada vez que el medico lo atienda
            Models.User user = _unitOfWork.User.GetFirstOrDefault(x => x.Id == id, null);
            user.LastDateAttended = DateTime.Now;
            _unitOfWork.User.Update(user);
            _unitOfWork.Save();

            //Añadiendo los objetos de datos del expediente medico del paciente

            if (medicalHistory.MedicalHistoryTreatments != null)
            {
                for (int j = 0; j < medicalHistory.MedicalHistoryTreatments.Count(); j++)
                {
                    var aux = medicalHistory.MedicalHistoryTreatments.ElementAt(j);
                    var treatment = _unitOfWork.HistoryTreatment.GetFirstOrDefault(u => u.MedicalHistoryId == aux.MedicalHistoryId, x => x.TreatmentId == aux.TreatmentId, includeProperties: "Treatments,Physicians");
                }
            }
            else
            {
                medicalHistory.MedicalHistoryTreatments = new List<MedicalHistory_Treatment>();
            }

            if (medicalHistory.MedicalHistorySufferings != null)
            {
                for (int j = 0; j < medicalHistory.MedicalHistorySufferings.Count(); j++)
                {
                    var aux = medicalHistory.MedicalHistorySufferings.ElementAt(j);
                    var suffering = _unitOfWork.HistorySuffering.GetFirstOrDefault(u => u.MedicalHistoryId == aux.MedicalHistoryId, x => x.SufferingId == aux.SufferingId, includeProperties: "Sufferings,Physicians");
                }
            }
            else
            {
                medicalHistory.MedicalHistorySufferings = new List<MedicalHistory_Suffering>();
            }

            if (medicalHistory.MedicalHistoryMedicines != null)
            {
                for (int j = 0; j < medicalHistory.MedicalHistoryMedicines.Count(); j++)
                {
                    var aux = medicalHistory.MedicalHistoryMedicines.ElementAt(j);
                    var medicine = _unitOfWork.HistoryMedicine.GetFirstOrDefault(
                        u => u.MedicalHistoryId == aux.MedicalHistoryId, x => x.MedicineId == aux.MedicineId,
                        includeProperties: "Medicines,Physicians");
                }
            }
            else
            {
                medicalHistory.MedicalHistoryMedicines = new List<MedicalHistory_Medicine>();
            }

            if (medicalHistory.MedicalNotes != null)
            {
                for (int i = 0; i<medicalHistory.MedicalNotes.Count; i++)
                {
                    var aux = medicalHistory.MedicalNotes.ElementAt(i);
                    var note = _unitOfWork.MedicalNote.GetFirstOrDefault(x => x.Id == aux.Id, null, includeProperties:"Physician");
                }
            }
            else
            {
                medicalHistory.MedicalNotes = new List<MedicalNote>();
            }

            if (medicalHistory.MedicalImages != null)
            {
                for (int i = 0; i < medicalHistory.MedicalImages.Count; i++)
                {
                    var aux = medicalHistory.MedicalImages.ElementAt(i);
                    var note = _unitOfWork.MedicalImage.GetFirstOrDefault(x => x.Id == aux.Id, null, includeProperties: "Physician");
                }
            }
            else
            {
                medicalHistory.MedicalImages = new List<MedicalImage>();
            }


            return View(medicalHistory);

        }
        #endregion

        #region API

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Physician.GetFirstOrDefault(x => x.Id == id, null);

            if (obj == null)
                return Json(new { success = false, message = "Error thile deleting" });


            var oldImageUrl = Path.Combine(_hostEnvironment.WebRootPath, obj.PicturePath);
            if (System.IO.File.Exists(oldImageUrl))
            {
                System.IO.File.Delete(oldImageUrl);
            }

            _unitOfWork.Physician.Remove(obj);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Deleted Successfully" });

            #endregion



        }
    }
}
