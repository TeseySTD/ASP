using Library.Models.Entities;

namespace Library.Models.DTO.InfoResponses;

public class SecondInfoResponse
{
    public List<Borrow> Debtors { get; set; }
    public string Type { get; set; }
}