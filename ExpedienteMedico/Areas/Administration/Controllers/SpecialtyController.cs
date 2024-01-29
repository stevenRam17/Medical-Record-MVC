using ExpedienteMedico.Models;
using ExpedienteMedico.Repository.IRepository;
using ExpedienteMedico.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpedienteMedico.Areas.Administration.Controllers
{

    [Area("Administration")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class SpecialtyController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostEnvironment;

        public SpecialtyController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Specialty> objSpecialtyList = _unitOfWork.Specialty.GetAll();
            return View(objSpecialtyList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Specialty obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Specialty.Add(obj);
                _unitOfWork.Save();
            }
            TempData["success"] = "Specialty created succesfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Specialty obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Specialty.Update(obj);
                _unitOfWork.Save();
            }

            TempData["success"] = "Specialty edited succesfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _unitOfWork.Specialty.GetFirstOrDefault(x => x.Id == id, null);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        
        public IActionResult Delete(int? id)
        {
            var speciality = _unitOfWork.Specialty.GetFirstOrDefault(x => x.Id == id, null);
            _unitOfWork.Specialty.Remove(speciality);
            _unitOfWork.Save();

            TempData["success"] = "Specialty deleted succesfully";
            return RedirectToAction("Index");
        }


        #region API

        public IActionResult GetAll()
        {
            var specialty = _unitOfWork.Specialty.GetAll();
            return Json(new { data = specialty, success = true });
        }
        #endregion
    }
}
