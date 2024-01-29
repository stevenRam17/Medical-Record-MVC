using ExpedienteMedico.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpedienteMedico.Areas.User.Controllers
{

    [Route("User/[controller]")]
    [ApiController]
    [EnableCors("GeneralPolicy")]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _db;
        private UserManager<IdentityUser> _userManager;

        public AuthController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _db = unitOfWork;
            _userManager = userManager;
        }

        //API 

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login(string email, string password)
        {
            IdentityUser user = _userManager.FindByEmailAsync(email).Result;
            bool isCorrect = _userManager.CheckPasswordAsync(user, password).Result;
            if (user != null && isCorrect)
            {
                return Json(new { data = user, success = true });
            }
            else
            {
                string errorData = "";
                if (user == null)
                {
                    errorData = "Usuario no encontrado";
                }
                else if (isCorrect == false)
                {
                    errorData = "Contraseña incorrecta";
                }
                return Json(new { data = errorData, success = false });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("userInfo")]
        public IActionResult UserInfo(string userId)
        {
            IdentityUser u = _userManager.FindByIdAsync(userId).Result;
            Models.User user  = (Models.User)u;
            if (user != null)
            {
                return Json(new { data = user, sucess = true });
            } else
            {
                return Json(new { data = "No se pudo encontrar el usuario", sucess = true });

            }
        }



    }
}
