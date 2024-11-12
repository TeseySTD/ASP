using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Library.Models.DTO;

public class LoginUserRequest
{
    [Required(ErrorMessage = "Введіть електронну пошту.")]
    [StringLength(50, ErrorMessage = "Пошта не може бути довше 50 символів.")]
    [EmailAddress(ErrorMessage = "Введіть коректну електронну пошту.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Пароль не може бути порожнім.")]
    [DataType(DataType.Password)]
    [Remote(action: "ValidatePassword", controller: "Validation", AdditionalFields = nameof(Email), ErrorMessage = "Невірний пароль.")]
    public string Password { get; set; } = null!;
}
