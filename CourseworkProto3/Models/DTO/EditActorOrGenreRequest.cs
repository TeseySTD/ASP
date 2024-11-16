using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Library.Models.DTO;

public class EditActorOrGenreRequest
{
    [Required(ErrorMessage = "Це поле не може бути порожним")]
    [Remote(action: "ValidateGenreOrActor", controller: "Validation", AdditionalFields = "Id,Type")]
    public string Name { get; set; }
    [Required]
    public string Type { get; set; }
    [Required]
    public int Id { get; set; }
}