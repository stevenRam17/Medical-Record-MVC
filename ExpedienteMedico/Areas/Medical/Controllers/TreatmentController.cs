using System.Security.Claims;
using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;
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
    public class TreatmentController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostEnvironment;
        private UserManager<IdentityUser> _userManager;

        public TreatmentController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            IEnumerable<Treatment> objTreatmentList = _unitOfWork.Treatment.GetAll();
            return View(objTreatmentList);
        }

        public IActionResult CreateForHistory(string id) //User id
        {

            MedicalHistory medicalHistory = _unitOfWork.MedicalHistory.GetFirstOrDefault(x => x.UserId == id, null,
                includeProperties: "MedicalHistoryTreatments");

            TreatmentVM vm = new TreatmentVM();

            vm.HistoryId = medicalHistory.UserId;
            vm.Treatment = null;

            return View(vm);
        }

        [HttpPost]
        public IActionResult CreateForHistory(TreatmentVM vm) //User id
        {
            if (ModelState.IsValid)
            {

                Treatment savedTreatment = null;
                var treatment = new Treatment() { Name = vm.Treatment.Name, Description = vm.Treatment.Description };
                _unitOfWork.Treatment.Add(treatment);
                _unitOfWork.Save();
                savedTreatment = _unitOfWork.Treatment.GetLast();


                Physician Physician =
                    _unitOfWork.Physician.GetByEmail(_userManager.FindByNameAsync(User.Identity.Name).Result.Email);

                var historyTreatment = new MedicalHistory_Treatment()
                {
                    MedicalHistoryId = vm.HistoryId,
                    TreatmentId = savedTreatment.Id,
                    PhysicianId = Physician.Id,
                    Physicians = Physician
                };

                _unitOfWork.HistoryTreatment.Add(historyTreatment);
                _unitOfWork.Save();
                TempData["success"] = "Treatment added successfully";

                string url = "/Medical/MedicalHistory/Upsert?id=" + vm.HistoryId;
                return Redirect(url);
            }
            else
            {
                return View(vm);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Treatment obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Treatment.Add(obj);
                _unitOfWork.Save();
            }
            TempData["success"] = "Treatment created succesfully";
            return RedirectToAction("Index");
        }

        public IActionResult Suspend(int? id)
        {
            var treatment = _unitOfWork.Treatment.GetFirstOrDefault(x => x.Id == id, null);
            if (treatment.IsSuspended == false)
            {
                treatment.IsSuspended = true;
                TempData["success"] = "Treatment suspended succesfully";
            }
            else
            {
                treatment.IsSuspended = false;
                TempData["success"] = "Treatment activated succesfully";
            }
            _unitOfWork.Treatment.Update(treatment);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Treatment obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Treatment.Update(obj);
                _unitOfWork.Save();
            }

            TempData["success"] = "Treatment edited succesfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _unitOfWork.Treatment.GetFirstOrDefault(x => x.Id == id, null);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        #region API

        public IActionResult GetAll()
        {
            var treatment = _unitOfWork.Treatment.GetAll();
            return Json(new { data = treatment, success = true });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var treatment = _unitOfWork.Treatment.GetFirstOrDefault(x => x.Id == id, null);

            if (treatment == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Treatment.Remove(treatment);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
            //return RedirectToAction("Index");
        }
        #endregion
    }
}
