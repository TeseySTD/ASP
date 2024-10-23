using System.ComponentModel.DataAnnotations;

namespace Library.Models.Entities;

public abstract class Genre
{
    public string Name { get; set; } = null!;
}

public class BookGenre : Genre
{
    [Key]
    public int GenreId { get; set; }
    public List<Book> Books { get; set; } = null!;
}

public class MusicGenre : Genre
{
    [Key]
    public int GenreId { get; set; }
    public List<Music> Music { get; set; } = null!;
}

public class MovieGenre : Genre
{
    [Key]
    public int GenreId { get; set; }
    public List<Movie> Movies { get; set; } = null!;
}

public class GameGenre : Genre
{
    [Key]
    public int GenreId { get; set; }
    public List<Game> Games { get; set; } = null!;
}