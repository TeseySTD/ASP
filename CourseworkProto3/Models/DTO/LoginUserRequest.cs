using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Library.Models.DTO;

public class LoginUserRequest
{
    [Required(ErrorMessage = "Login is required.")]
    [StringLength(50, ErrorMessage = "Login cannot be longer than 50 characters.")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [Remote(action: "ValidatePassword", controller: "Home", AdditionalFields = nameof(Login))]
    public string Password { get; set; } = null!;
}
