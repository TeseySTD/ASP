using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseworkProto1.Models.Entities;

public class Game
{
    [Key]
    public int GameId { get; set; }

    [ForeignKey("Disc")]
    public int DiscId { get; set; }
    public Disc Disc { get; set; }

    public int Platform { get; set; }
    public string Developer { get; set; }
}
