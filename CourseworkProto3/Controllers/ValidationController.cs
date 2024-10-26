using System.Text.RegularExpressions;
using Library.Data.Repo;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class ValidationController : Controller
    {
        private readonly UserRepository _userRepository;

        public ValidationController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: Validation
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateEmail(string email){
            if(_userRepository.IsEmailTaken(email)){
                return Json(false);
            }
            return Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidatePassword(string email, string password)
        {

            if (_userRepository.IsPasswordCorrect(email, password))
            {
                return Json(true);
            }
            else
            {
                return Json($"Невірний пароль для {email}.");
            }
        }

        public IActionResult ValidateEndDate(DateTime BorrowEndDate){
            if (BorrowEndDate > DateTime.Now)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

    }
}
