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
    public class MedicalImageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostEnvironment;
        UserManager<IdentityUser> _userManager;

        public MedicalImageController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            IEnumerable<MedicalImage> objMedicalImageList = _unitOfWork.MedicalImage.GetAll();
            return View(objMedicalImageList);
        }

        public IActionResult CreateForHistory(string id)
        {
            MedicalImageVM objMedicalImage = new MedicalImageVM();
            objMedicalImage.Image = new MedicalImage();


            int PhysicianId =
                _unitOfWork.Physician.GetByEmail(_userManager.FindByNameAsync(User.Identity.Name).Result.Email).Id;
            objMedicalImage.Image.PhysicianId = PhysicianId;
            objMedicalImage.Image.MedicalHistoryId = id;

            return View(objMedicalImage);
        }

        [HttpPost]
        public IActionResult CreateForHistory(MedicalImageVM objMedicalImage, IFormFile? file, IFormFile? filePdf)
        {
            if (ModelState.IsValid)
            {

                #region imageManage

                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\MedicalImages");
                    var extension = Path.GetExtension(file.FileName);

                    if (objMedicalImage.Image.ImageUrl != null)
                    {
                        var oldImageUrl = Path.Combine(wwwRootPath, objMedicalImage.Image.ImageUrl);
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

                    objMedicalImage.Image.ImageUrl = @"images\MedicalImages\" + fileName + extension;
                }

                #endregion

                #region pdfManage

                if (filePdf != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"pdfs\MedicalPdfs");
                    var extension = Path.GetExtension(filePdf.FileName);

                    if (objMedicalImage.Image.PdfUrl != null)
                    {
                        var oldPdfUrl = Path.Combine(wwwRootPath, objMedicalImage.Image.PdfUrl);
                        if (System.IO.File.Exists(oldPdfUrl))
                        {
                            System.IO.File.Delete(oldPdfUrl);
                        }
                    }

                    using (var fileStreams =
                           new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        filePdf.CopyTo(fileStreams);
                    }

                    objMedicalImage.Image.PdfUrl = @"pdfs\MedicalPdfs\" + fileName + extension;
                }

                #endregion

                #region medicalImage

                _unitOfWork.MedicalImage.Add(objMedicalImage.Image);
                _unitOfWork.Save();
                TempData["success"] = "Medical image added succesfully";

                string url = "/Medical/MedicalHistory/Upsert?id=" + objMedicalImage.Image.MedicalHistoryId;
                return Redirect(url);
                #endregion

            }
            else
            {
                return View(objMedicalImage);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MedicalImage obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.MedicalImage.Update(obj);
                _unitOfWork.Save();
            }

            TempData["success"] = "Medical Image edited succesfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _unitOfWork.MedicalImage.GetFirstOrDefault(x => x.Id == id, null);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Delete(int? id)
        {
            var medicalImage = _unitOfWork.MedicalImage.GetFirstOrDefault(x => x.Id == id, null);

            if (medicalImage == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.MedicalImage.Remove(medicalImage);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
            //return RedirectToAction("Index");
        }


        #region API

        public IActionResult GetAll()
        {
            var medicalImage = _unitOfWork.MedicalImage.GetAll();
            return Json(new { data = medicalImage, success = true });
        }

        public IActionResult Get(string id)
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
        #endregion
    }
}
