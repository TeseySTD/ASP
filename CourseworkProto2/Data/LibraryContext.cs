using CourseworkProto1.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseworkProto1.Data;

public class LibraryContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Disc> Discs { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Music> Music { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Borrow> Borrows { get; set; }
    public DbSet<Rental> Rentals { get; set; }

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


    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     // Каскадне видалення для MusicDisc -> Disc
    //     modelBuilder.Entity<MusicDisc>()
    //         .HasOne(md => md.Disc)
    //         .WithOne(d => d.MusicDisc)
    //         .HasForeignKey<MusicDisc>(md => md.DiscId)
    //         .OnDelete(DeleteBehavior.Cascade);

    //     // Каскадне видалення для Movie -> Disc
    //     modelBuilder.Entity<Movie>()
    //         .HasOne(m => m.Disc)
    //         .WithOne(d => d.Movie)
    //         .HasForeignKey<Movie>(m => m.DiscId)
    //         .OnDelete(DeleteBehavior.Cascade);

    //     // Каскадне видалення для Game -> Disc
    //     modelBuilder.Entity<Game>()
    //         .HasOne(g => g.Disc)
    //         .WithOne(d => d.Game)
    //         .HasForeignKey<Game>(g => g.DiscId)
    //         .OnDelete(DeleteBehavior.Cascade);

    //     // Каскадне видалення для Disc -> Product
    //     modelBuilder.Entity<Disc>()
    //         .HasOne(d => d.Product)
    //         .WithOne(p => p.Disc)
    //         .HasForeignKey<Disc>(d => d.ProductId)
    //         .OnDelete(DeleteBehavior.Cascade);
    // }


}