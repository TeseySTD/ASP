using Library.Models.Entities;

namespace Library.Models.DTO.InfoResponses;

public class SeventhInfoResponse{
    public User? ConcreteUser { get; set; } = null;

    public List<User> DebtorsOfBooks { get; set; } = new List<User>();
}