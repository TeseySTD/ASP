using System.ComponentModel.DataAnnotations;
using Library.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Library.Models.DTO;

public class TableColumnDTO
{
    [Required(ErrorMessage = "Назва поля не може бути пустою")]
    [StringLength(50, ErrorMessage = "Назва поля повинна містити від 1 до 50 символів")]
    [Remote(action:"ValidateUniqueColumnName", controller:"Validation", AdditionalFields = nameof(TableName), ErrorMessage ="Така назва поля вже існує")]
    public string ColumnName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Тип даних не може бути пустим")]
    public TableDataType DataType { get; set; } = TableDataType.VARCHAR;
    [Required(ErrorMessage = "Чи може поле бути пустим")]
    public bool IsNullable { get; set; }

    public string TableName { get; set; }
}

public class TableDTO
{
    [Required(ErrorMessage = "Назва таблиці не може бути пустою")]
    public string TableName { get; set; } = string.Empty;
    public List<TableColumnDTO> Columns { get; set; } = new List<TableColumnDTO>();
}