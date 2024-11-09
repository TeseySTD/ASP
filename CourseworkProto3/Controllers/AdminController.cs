using Library.Models.DTO;
using Library.Models.Entities;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Controllers
{
    public class AdminController : Controller
    {
        private readonly TableService _tableService;

        public AdminController(TableService tableService)
        {
            _tableService = tableService;
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

        [HttpGet]
        public async Task<IActionResult> AddColumn(string tableName){
            ViewBag.TableName = tableName;
            return View();
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
        public IActionResult AddTable(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTable(AddTableRequest table){
            await _tableService.AddTable(table);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTable(string tableName){
            if(!await _tableService.TableExists(tableName))
                return NotFound();
                
            await _tableService.DeleteTable(tableName);
            return RedirectToAction("Index");
        }

    }
}
