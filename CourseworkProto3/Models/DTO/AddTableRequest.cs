using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Library.Models.DTO;
public class AddTableRequest
{
    [Required(ErrorMessage = "Назва первинного ключа обов'язкова")]
    [StringLength(50, ErrorMessage = "Назва первинного ключа не повинна перевищувати 50 символів")]
    public string PrimaryKeyName { get; set; }= null!;

    [Required(ErrorMessage = "Назва таблиці обов'язкова")]
    [StringLength(50, ErrorMessage = "Назва таблиці не повинна перевищувати 50 символів")]
    [Remote("ValidateUniqueTableName", "Validation", ErrorMessage = "Таблиця з такою назвою вже існує")]
    public string TableName { get; set;} = null!;
}