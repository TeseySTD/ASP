using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Library.Models.DTO;

public class OrderDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Обов'язкове поле")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "Обов'язкове поле")]
    [Remote("ValidateEndDate", "Validation", ErrorMessage = "Дата повинна бути більшою за поточну")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Обов'язкове поле")]
    [Range(1, 10000, ErrorMessage = "Значення має бути від 1 до 10000")]
    public decimal PaymentAmount { get; set; }
}