using ExpedienteMedico.Models;
using ExpedienteMedico.Models.ViewModels;
using ExpedienteMedico.Repository.IRepository;
using ExpedienteMedico.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace ExpedienteMedico.Areas.Medical.Controllers
{
    [Area("Medical")]
    [Authorize(Roles = Roles.Role_Admin + "," + Roles.Role_Physician)]
    public class MedicalNoteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostEnvironment;
        private UserManager<IdentityUser> _userManager;

        public MedicalNoteController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        public IActionResult CreateForHistory(string id)
        {
            MedicalNoteVM objMedicalNote = new MedicalNoteVM();
            objMedicalNote.Note = new MedicalNote();


            int PhysicianId =
                _unitOfWork.Physician.GetByEmail(_userManager.FindByNameAsync(User.Identity.Name).Result.Email).Id;
            objMedicalNote.Note.PhysicianId = PhysicianId;
            objMedicalNote.Note.MedicalHistoryId = id;

            return View(objMedicalNote);
        }

        [HttpPost]
        public IActionResult CreateForHistory(MedicalNoteVM vm)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.MedicalNote.Add(vm.Note);
                _unitOfWork.Save();
                TempData["success"] = "Medical note added succesfully";
                string url = "/Medical/MedicalHistory/Upsert?id=" + vm.Note.MedicalHistoryId;
                return Redirect(url);
            }
            else
            {
                return View(vm);
            }
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _unitOfWork.MedicalNote.GetFirstOrDefault(x => x.Id == id, null);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Delete(int? id)
        {
            var medicalNote = _unitOfWork.MedicalNote.GetFirstOrDefault(x => x.Id == id, null);

            if (medicalNote == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.MedicalNote.Remove(medicalNote);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
            //return RedirectToAction("Index");
        }


        #region API

        public IActionResult GetAll()
        {
            var medicalNote = _unitOfWork.MedicalNote.GetAll();
            return Json(new { data = medicalNote, success = true });
        }

        public IActionResult Get(string id)
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
        #endregion
    }
}

