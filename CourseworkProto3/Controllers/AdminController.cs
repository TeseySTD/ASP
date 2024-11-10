using Library.Data.Repo;
using Library.Models.DTO;
using Library.Models.Entities;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class AdminController : Controller
    {
        private readonly TableService _tableService;
        private readonly UserRepository _userRepository;

        public AdminController(TableService tableService, UserRepository userRepository)
        {
            _tableService = tableService;
            _userRepository = userRepository;
        }

        // Показ списку полів таблиці
        public async Task<IActionResult> Index()
        {
            var tableNames = await _tableService.GetTableNamesAsync();
            List<TableDTO> tables = new List<TableDTO>();
            foreach (var tableName in tableNames)
            {
                tables.Add(await _tableService.GetTableAsync(tableName));
            }

            return View(tables);
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userRepository.GetAllUsers();
            List<EditUserRequest> userRequests = new List<EditUserRequest>();
            foreach (var user in users)
            {
                userRequests.Add(new EditUserRequest
                {
                    UserId = user.UserId,
                    Login = user.Login,
                    Email = user.Email,
                    Password = user.Password,
                    Role = user.Role,
                    Gender = user.Gender
                });
            }

            return View(userRequests);
        }

        [HttpGet]
        public async Task<IActionResult> AddColumn(string tableName)
        {
            var column = new TableColumnDTO()
            {
                TableName = tableName,
            };
            return View(column);
        }

        // Додавання нового поля
        [HttpPost]
        public async Task<IActionResult> AddColumn(string tableName, TableColumnDTO column)
        {
            await _tableService.AddColumn(tableName, column);
            return RedirectToAction("Index");
        }

        // Видалення поля
        [HttpPost]
        public async Task<IActionResult> DeleteColumn(string tableName, string columnName)
        {
            await _tableService.DeleteColumn(tableName, columnName);
            return RedirectToAction("Index");
        }

        // Зміна типу даних поля
        [HttpPost]
        public async Task<IActionResult> EditColumn(string tableName, TableColumnDTO column)
        {
            await _tableService.EditColumn(tableName, column);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddTable()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTable(AddTableRequest table)
        {
            await _tableService.AddTable(table);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTable(string tableName)
        {
            if (!await _tableService.TableExists(tableName))
                return NotFound();

            await _tableService.DeleteTable(tableName);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int userId){
            if(!_userRepository.IsUser(userId))
                return NotFound();

            var user = await _userRepository.GetUserByIdAsync(userId);
            var editRequest = new EditUserRequest{
                UserId = userId,
                Login = user.Login,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Gender = user.Gender
            };

            return View(editRequest);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserRequest request){
            if(!_userRepository.IsUser(request.UserId))
                return NotFound();

            await _userRepository.UpdateUser(request);
            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId){
            if(!_userRepository.IsUser(userId))
                return NotFound();

            await _userRepository.DeleteUser(userId);
            return RedirectToAction("Users");
        }
    }
}
