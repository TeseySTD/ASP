using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Entities;

public class Game
{
    [Key]
    public int GameId { get; set; }

    [ForeignKey("Disc")]
    public int DiscId { get; set; }
    public Disc Disc { get; set; } = null!;

    public List<GameGenre> Genre { get; set; } = null!;
    public string Developer { get; set; } = null!;
    public string Publisher { get; set; } = null!;
}
