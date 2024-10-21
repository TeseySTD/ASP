using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication0.Models;

namespace WebApplication0.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        throw new Exception("Test exception");
        // return StatusCode(505);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int statusCode)
    {
        return View(statusCode);
    }

}