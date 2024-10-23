using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Entities;

public class Music
{
    [Key]
    public int MusicId { get; set; }

    [ForeignKey("Disc")]
    public int DiscId { get; set; }
    public Disc Disc { get; set; }

    public string Artist { get; set; }
    public List<MusicGenre> Genre { get; set; }
    public int TrackCount { get; set; }
}
