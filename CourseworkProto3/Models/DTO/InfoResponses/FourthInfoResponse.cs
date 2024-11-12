using Library.Models.Entities;

namespace Library.Models.DTO.InfoResponses;

public class FourthInfoResponse{
    public List<User> NotOwe{ get; set; }
    public List<User> Owe{ get; set; }
}