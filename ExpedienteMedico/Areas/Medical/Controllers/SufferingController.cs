using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;
using ExpedienteMedico.Models.ViewModels;
using ExpedienteMedico.Repository.IRepository;
using ExpedienteMedico.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpedienteMedico.Areas.Medical.Controllers
{
    [Area("Medical")]
    [Authorize(Roles = Roles.Role_Admin + "," + Roles.Role_Physician)]
    public class SufferingController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostEnvironment;
        private UserManager<IdentityUser> _userManager;

        public SufferingController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            IEnumerable<Suffering> objSufferingList = _unitOfWork.Suffering.GetAll();
            return View(objSufferingList);
        }

        public IActionResult CreateForHistory(string id) //User id
        {

            MedicalHistory medicalHistory = _unitOfWork.MedicalHistory.GetFirstOrDefault(x => x.UserId == id, null,
                includeProperties: "MedicalHistorySufferings");

            SufferingVM vm = new SufferingVM();

            vm.HistoryId = medicalHistory.UserId;
            vm.Suffering = null;

            return View(vm);
        }

        [HttpPost]
        public IActionResult CreateForHistory(SufferingVM vm) //User id
        {
            Suffering savedSuffering = null;
            if (ModelState.IsValid)
            {
                var Suffering = new Suffering() { Name = vm.Suffering.Name, Description = vm.Suffering.Description };
                _unitOfWork.Suffering.Add(Suffering);
                _unitOfWork.Save();
                savedSuffering = _unitOfWork.Suffering.GetLast();


                Physician Physician =
                    _unitOfWork.Physician.GetByEmail(_userManager.FindByNameAsync(User.Identity.Name).Result.Email);

                var historySuffering = new MedicalHistory_Suffering()
                {
                    MedicalHistoryId = vm.HistoryId,
                    SufferingId = savedSuffering.Id,
                    PhysicianId = Physician.Id, 
                    Physicians = Physician
                };

                _unitOfWork.HistorySuffering.Add(historySuffering);
                _unitOfWork.Save();
                TempData["success"] = "Suffering added successfully";
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
        public IActionResult Create(Suffering obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Suffering.Add(obj);
                _unitOfWork.Save();
            }
            TempData["success"] = "Suffering created succesfully";
            return RedirectToAction("Index");
        }

        public IActionResult Suspend(int? id)
        {
            var suffering = _unitOfWork.Suffering.GetFirstOrDefault(x => x.Id == id, null);
            if (suffering.IsSuspended == false)
            {
                suffering.IsSuspended = true;
                TempData["success"] = "suffering suspended succesfully";
            }
            else
            {
                suffering.IsSuspended = false;
                TempData["success"] = "suffering activated succesfully";
            }
            _unitOfWork.Suffering.Update(suffering);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Suffering obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Suffering.Update(obj);
                _unitOfWork.Save();
            }

            TempData["success"] = "Suffering edited succesfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _unitOfWork.Suffering.GetFirstOrDefault(x => x.Id == id, null);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }



        #region API

        public IActionResult GetAll()
        {
            var suffering = _unitOfWork.Suffering.GetAll();
            return Json(new { data = suffering, success = true });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var suffering = _unitOfWork.Suffering.GetFirstOrDefault(x => x.Id == id, null);

            if (suffering == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Suffering.Remove(suffering);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
            //return RedirectToAction("Index");
        }

        #endregion
    }
}
