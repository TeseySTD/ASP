using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Library.Models.DTO;

public class ForgotPasswordDTO
{
    [Required(ErrorMessage = "Логін не може бути порожнім.")]
    [StringLength(50, ErrorMessage = "Логін не може бути довше ніж 50 символів.")]
    [Remote(action: "ValidateLoginInDb", controller: "Validation", ErrorMessage ="Такого логіну нема в системі.")]
    public string Login { get; set; }

    public List<string> Passwords { get; set; } = new List<string>();
}   