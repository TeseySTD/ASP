using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.Data;
using Microsoft.EntityFrameworkCore;
using Library.Models.Entities;
using Library.Data.Repo;
using Library.Services;
using Microsoft.AspNetCore.Authorization;
using Library.Models.DTO;

namespace Library.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserRepository _userRepository;
    private readonly UserService _userService;

    public HomeController(ILogger<HomeController> logger, 
                        UserRepository userRepository,
                        UserService userService)
    {
        _logger = logger;
        _userRepository = userRepository;
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View();
    }


    [Authorize(Policy = "Administrator")]
    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        await _userService.Register(request.Login, request.Password, request.Gender);
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginUserRequest request){

        var token = await _userService.Login(request.Login, request.Password);
        if(token == null)
            throw new Exception("Wrong login or password");

        Response.Cookies.Append("access-cookie", token);  
        return RedirectToAction("Index");
    }

    [Authorize]
    public IActionResult Logout(){
        Response.Cookies.Delete("access-cookie");
        return RedirectToAction("Index");
    }

    [AcceptVerbs("Get", "Post")]
    public IActionResult ValidateLogin(string login){
        if(_userRepository.IsLoginTaken(login)){
            return Json("Login is already taken.");
        }
        return Json(true);
    }

    [AcceptVerbs("Get", "Post")]
    public IActionResult ValidatePassword(string login, string password)
    {

        if (_userRepository.IsPasswordCorrect(login, password))
        {
            return Json(true);
        }
        else
        {
            return Json($"Incorrect password for user {login}.");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
