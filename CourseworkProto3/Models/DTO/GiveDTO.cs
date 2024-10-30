using System.ComponentModel.DataAnnotations;
using Library.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.Models.DTO;

public class GiveDTO
{
    public int ProductId { get; set; }
    public int LenderId { get; set; }

    [Required(ErrorMessage = "Обов'язкове поле.")]
    [Remote("IsEmailAvailableForCurrentUser", "Validation")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Обов'язкове поле")]
    [DataType(DataType.Date)]
    public DateTime BorrowStartDate { get; set; }

    [Required(ErrorMessage = "Обов'язкове поле")]
    [Remote("ValidateEndDate", "Validation", ErrorMessage = "Дата повинна бути більшою за поточну")]
    [DataType(DataType.Date)]
    public DateTime BorrowEndDate { get; set; } 

}