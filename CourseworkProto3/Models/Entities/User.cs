using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Library.Models.Entities
{

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Login is required.")]
        [StringLength(50, ErrorMessage = "Login cannot be longer than 50 characters.")]
        [Remote(action: "ValidateLogin", controller: "Home")]
        public string Login { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Access rights are required.")]
        public Roles AccessRights { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender type.")]
        public Gender Gender { get; set; }
    }

}
