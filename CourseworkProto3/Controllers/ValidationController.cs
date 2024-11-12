using System.Text.RegularExpressions;
using Library.Data.Repo;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class ValidationController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly TableService _tableService;

        public ValidationController(UserRepository userRepository, TableService tableService)
        {
            _userRepository = userRepository;
            _tableService = tableService;
        }

        // GET: Validation
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateEmail(string email)
        {
            if (_userRepository.IsEmailTaken(email))
            {
                return Json(false);
            }
            return Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsEmailAvailableForCurrentUser(string email)
        {
            var token = HttpContext.Request.Cookies["access-cookie"];
            if (!_userRepository.IsEmailTaken(email))
                return Json("Такої пошти не існує в бібліотеці.");
            else if (_userRepository.GetUserByToken(token).Email == email)
                return Json("Ви не можете використовувати вашу пошту.");
            else
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
        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateEndDate(DateTime BorrowEndDate)
        {
            if (BorrowEndDate > DateTime.Now)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public async Task<IActionResult> ValidateUniqueTableName(string tableName)
        {
            var tables = await _tableService.GetTableNamesAsync();
            return tables.Any(t => t == tableName) ? Json(false) : Json(true);
        }

        public async Task<IActionResult> ValidateUniqueColumnName(string tableName, string columnName)
        {
            return await _tableService.ColumnExists(tableName, columnName) ? Json(false) : Json(true);
        }

        public async Task<IActionResult> ValidateUniqueEmailExceptUser(string email, int userId)
        {
            if (await _userRepository.IsEmailTakenExceptUser(email, userId))
            {
                return Json(false);
            }
            else
                return Json(true);
        }

        public IActionResult ValidateLoginInDb(string login)
        {
            return Json(_userRepository.IsLoginTaken(login));
        }
    }
}
