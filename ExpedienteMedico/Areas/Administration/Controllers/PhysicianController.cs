
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Windows;
using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;
using ExpedienteMedico.Models.ViewModels;
using ExpedienteMedico.Repository.IRepository;
using ExpedienteMedico.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ExpedienteMedico.Areas.Administration.Controllers
{

    [Area("Administration")]
    [Authorize(Roles = Roles.Role_Admin)]

    public class PhysicianController : Controller
    {

        #region HTTP GET POST

        private readonly IUnitOfWork _unitOfWork;
        private UserManager<IdentityUser> _userManager;
        private IWebHostEnvironment _hostEnvironment;

        public PhysicianController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;

        }

        public IActionResult Index()
        {
            IEnumerable<Physician> objPhysicianList = _unitOfWork.Physician.GetAll(includeProperties: "PhysicianSpecialties");
            return View(objPhysicianList);
        }

        //GET ********************************
        public IActionResult Upsert(int? id)   //Update + Insert
        {
            var physician = new Physician();
            physician.Id = 0;

            var vm = new PhysicianCreateVM()
            {
                Physician = new(),
                Specialties = _unitOfWork.Specialty.GetAll().Select(i => new SpecialtySelectVM()
                {
                    SpecialtyId = i.Id,
                    Name = i.Name,
                    IsSelected = false
                }).ToList()
            };


            if (id == 0 || id == null)
            {
                return View(vm);
            }
            else
            {
                vm.Physician = _unitOfWork.Physician.GetFirstOrDefault(u => u.Id == id, null);
                return View(vm);
            }
        }

        //POST **********************************
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(PhysicianCreateVM obj, IFormFile? file)
        {
            bool IsGetted = false;
            if (ModelState.IsValid)
            {

                #region imageManage
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\Physicians");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Physician.PicturePath != null)
                    {
                        var oldImageUrl = Path.Combine(wwwRootPath, obj.Physician.PicturePath);
                        if (System.IO.File.Exists(oldImageUrl))
                        {
                            System.IO.File.Delete(oldImageUrl);
                        }
                    }

                    using (var fileStreams =
                           new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    obj.Physician.PicturePath = @"images\Physicians\" + fileName + extension;
                }

                #endregion

                var Physician = obj.Physician;

                if (obj.Physician.Id == 0)
                {
                    //Aqui se obtiene el id de usuario mediante el email asociado, para que en un futuro el medico se logee
                    Models.User user = _unitOfWork.User.GetFirstOrDefault(x => x.Email == Physician.Email, null);
                    if (user != null)
                    {
                        if (_userManager.IsInRoleAsync(user, Roles.Role_Physician).Result)
                        {
                            Physician.UserId = user.Id;
                        }
                    }

                    try
                    {
                        _unitOfWork.Physician.Add(obj.Physician);
                        _unitOfWork.Save();
                        IsGetted = true;
                        Physician = _unitOfWork.Physician.GetLast();
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = "The Physician must be associated with a physician login or the email is already taken";
                        return RedirectToAction("Index");
                    }
                }

                #region specialtiesManage


                foreach (var selectedSpecialty in obj.Specialties.Where(c => c.IsSelected))
                {
                    var specialty = new Specialty { Id = selectedSpecialty.SpecialtyId, Name = selectedSpecialty.Name };

                    var physicianSpecialty = new PhysicianSpecialty
                    {
                        PhysicianId = Physician.Id,
                        SpecialtyId = specialty.Id
                    };

                    var physicianSpecialtyAux = _unitOfWork.PhysicianSpecialty.GetFirstOrDefault(
                        u => u.SpecialtyId == selectedSpecialty.SpecialtyId, x => x.PhysicianId == Physician.Id);
                    if (physicianSpecialtyAux == null)
                    {
                        Physician.PhysicianSpecialties.Add(physicianSpecialty);
                        _unitOfWork.Physician.Add(Physician);
                    }
                }

                foreach (var selectedSpecialty in obj.Specialties.Where(c => !c.IsSelected))
                {
                    var physicianSpecialtyAux = _unitOfWork.PhysicianSpecialty.GetFirstOrDefault(
                        u => u.SpecialtyId == selectedSpecialty.SpecialtyId, x => x.PhysicianId == Physician.Id);
                    if (physicianSpecialtyAux != null)
                    {
                        _unitOfWork.PhysicianSpecialty.Remove(physicianSpecialtyAux);
                    }
                }

                #endregion

                _unitOfWork.Physician.Update(Physician);
                if (!IsGetted)
                    TempData["success"] = "Physician updated successfully";
                else
                    TempData["success"] = "Physician saved successfully";

                _unitOfWork.Save();

            }
            return RedirectToAction("Index");
        }

        #endregion

        #region API

        [HttpGet]
        public IActionResult GetAll()
        {
            var PhysicianList = _unitOfWork.Physician.GetAll(includeProperties: "PhysicianSpecialties");
            for (int i = 0; i < PhysicianList.Count(); i++)
            {
                var obj = PhysicianList.ElementAt(i);
                for (int j = 0; j < obj.PhysicianSpecialties.Count(); j++)
                {
                    var aux = obj.PhysicianSpecialties.ElementAt(j);
                    var physicianSpecialty = _unitOfWork.PhysicianSpecialty.GetFirstOrDefault(u => u.SpecialtyId == aux.SpecialtyId, x => x.PhysicianId == aux.PhysicianId, includeProperties: "Specialty");
                }
            }

            return Json(new { data = PhysicianList, success = true });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Physician.GetFirstOrDefault(x => x.Id == id, null);

            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

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
