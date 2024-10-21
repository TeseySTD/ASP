using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Entities;

public class Game
{
    [Key]
    public int GameId { get; set; }

    [ForeignKey("Disc")]
    public int DiscId { get; set; }
    public Disc Disc { get; set; }

    public GamePlatform Platform { get; set; }
    public GameGenre Genre { get; set; }
    public string Developer { get; set; }
}
