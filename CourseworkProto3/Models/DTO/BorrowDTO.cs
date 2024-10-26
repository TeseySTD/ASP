using System.ComponentModel.DataAnnotations;
using Library.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.Models.DTO;

public class BorrowDto
{
    public int ProductId { get; set; }
    public ProductType ProductType { get; set; }

    [Required(ErrorMessage = "Обов'язкове поле")]
    [DataType(DataType.Date)]
    public DateTime BorrowStartDate { get; set; }

    [Required(ErrorMessage = "Обов'язкове поле")]
    [Remote("ValidateEndDate", "Validation", ErrorMessage = "Дата повинна бути більшою за поточну")]
    [DataType(DataType.Date)]
    public DateTime BorrowEndDate { get; set; }
    public int BorrowerId { get; set; }
    public int LenderId { get; set; }
}