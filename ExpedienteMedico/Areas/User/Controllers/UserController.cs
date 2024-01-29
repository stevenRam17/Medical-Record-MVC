using ExpedienteMedico.Repository.IRepository;
using ExpedienteMedico.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpedienteMedico.Areas.User.Controllers
{

    [Area("User")]
    [Authorize(Roles = Roles.Role_Admin + "," + Roles.Role_Physician)]
    public class UserController : Controller
    {
        private IUnitOfWork _db;
        private UserManager<IdentityUser> _userManager;

        public UserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _db = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _db.User.GetAll();
            if (User.IsInRole(Roles.Role_Physician))
            {
                IList<IdentityUser> usersAux = _userManager.GetUsersInRoleAsync(Roles.Role_Patient).Result;
                List<Models.User> usersList = new List<Models.User>();
                foreach (var user in usersAux)
                {
                    usersList.Add(_db.User.GetFirstOrDefault(x => x.Id == user.Id, null));
                }
            }
            return View(users);
        }

        //GET ********************************
        public IActionResult Edit(string? id)   //Update + Insert
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _db.User.GetFirstOrDefault(x => x.Id == id, null);
            //var categoryFromDb = _db.Categories.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        //POST **********************************
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Models.User obj)
        {
            if (ModelState.IsValid)
            {
                _db.User.Update(obj);
                _db.Save();
                TempData["success"] = "User Updated successfully";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Attend(Models.User obj)
        {
            if (ModelState.IsValid)
            {
                _db.User.Update(obj);
                _db.Save();
                TempData["success"] = "User Updated successfully";
            }
            return RedirectToAction("Index");
        }

        //POST
        [HttpPost]
        public IActionResult Banned(string? id)
        {
            if (id != null)
            {
                _userManager.SetLockoutEnabledAsync(_db.User.GetFirstOrDefault(x => x.Id == id, null), true);
                _userManager.SetLockoutEndDateAsync(_db.User.GetFirstOrDefault(x => x.Id == id, null), DateTimeOffset.MaxValue);
            }
            _db.Save();
            return Json(new { success = true, message = "Banned Successfully" });
        }

        //POST
        [HttpPost]
        public IActionResult Unbanned(string? id)
        {
            if (id != null)
            {
                _userManager.SetLockoutEnabledAsync(_db.User.GetFirstOrDefault(x => x.Id == id, null), false);
                _userManager.SetLockoutEndDateAsync(_db.User.GetFirstOrDefault(x => x.Id == id, null), null);
            }
            _db.Save();
            return Json(new { success = true, message = "Unbanned Successfully" });
        }

  

        #region API

        [HttpGet]
        public IActionResult GetAll()   
        {
            var users = _db.User.GetAll();
            return Json(new { data = users, success = true });
        }

        [HttpGet]
        public IActionResult GetAllMedical()
        {
            IList<IdentityUser> users = _userManager.GetUsersInRoleAsync(Roles.Role_Patient).Result;
            List<Models.User> usersList = new List<Models.User>();
            foreach (var user in users)
            {
                usersList.Add(_db.User.GetFirstOrDefault(x => x.Id == user.Id, null));
            }
            return Json(new { data = usersList, success = true });
        }


        #endregion
    }
}
