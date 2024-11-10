using System;
using System.ComponentModel.DataAnnotations;
using Library.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.Models.DTO;

public class EditUserRequest
{
    [Required(ErrorMessage = "Id не може бути порожнім.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Логін не може бути порожнім.")]
    [StringLength(50, ErrorMessage = "Логін не може бути довше ніж 50 символів.")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Введіть електронну пошту.")]
    [StringLength(50, ErrorMessage = "Пошта не може бути довшою за 50 символів.")]
    [EmailAddress(ErrorMessage = "Некоректно введена пошта.")]
    [Remote(action: "ValidateUniqueEmailExceptUser", controller: "Validation", AdditionalFields = nameof(UserId), ErrorMessage = "Пошта вже використовується.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Пароль не може бути порожнім.")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Пароль повинен бути не менше 4 і не більше 100 символів.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Роль не може бути порожньою.")]
    [EnumDataType(typeof(Roles), ErrorMessage = "Некоректна роль.")]
    public Roles Role { get; set; }

    [Required(ErrorMessage = "Стать не може бути порожньою.")]
    [EnumDataType(typeof(Gender), ErrorMessage = "Некоректна стать.")]
    public Gender Gender { get; set; }
}
