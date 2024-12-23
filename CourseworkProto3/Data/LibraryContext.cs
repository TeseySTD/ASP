using System.Security.Permissions;
using Library.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public class LibraryContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Disc> Discs { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Music> Music { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookGenre> BookGenres { get; set; }
    public DbSet<GameGenre> GameGenres { get; set; }
    public DbSet<MusicGenre> MusicGenres { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Borrow> Borrows { get; set; }
    public DbSet<Order> Orders { get; set; }

    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }
    public LibraryContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=1234567890;database=CourseWorkProto1;",
                                new MySqlServerVersion(new Version(8, 0, 34)));
    }

}