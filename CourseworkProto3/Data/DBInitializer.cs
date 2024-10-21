using Library.Models.Entities;

namespace Library.Data;

public static class DBInitializer
{
    public static void Initialize(bool recreate = true)
    {
        using (LibraryContext context = new LibraryContext())
        {
            // Recreate the database
            if(recreate)
                context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Add Users
            var users = new List<User>
            {
                new User { UserId = 1, Login = "John Doe", Password = "pass", Gender = Gender.Male, AccessRights = Roles.Owner },
                new User { UserId = 2, Login = "Jane Smith", Password = "pass", Gender = Gender.Female, AccessRights = Roles.Administrator }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            // Add Products
            var products = new List<Product>
            {
                new Product { ProductId = 1, Title = "The Great Gatsby", ProductType = ProductType.Book},
                new Product { ProductId = 2, Title = "Inception", ProductType = ProductType.Disc},
                new Product { ProductId = 3, Title = "The Dark Side of the Moon", ProductType = ProductType.Disc},
                new Product { ProductId = 4, Title = "Call of Duty", ProductType = ProductType.Disc}
            };
            products[0].Owner = users[0];
            products[1].Owner = users[0];
            products[2].Owner = users[1];
            products[3].Owner = users[1];

            context.Products.AddRange(products);
            context.SaveChanges();

            // Add Books
            var books = new List<Book>
            {
                new Book { BookId = 1, ProductId = 1, Author = "F. Scott Fitzgerald", Genre = BookGenre.Fiction, PublicationYear = 1925 }
            };
            context.Books.AddRange(books);
            context.SaveChanges();

            // Add Discs
            var discs = new List<Disc>
            {
                new Disc { DiscId = 1, ProductId = 2, Format = DiscFormat.BluRay, Year = 2010, DiscType = DiscType.Movie },
                new Disc { DiscId = 2, ProductId = 3, Format = DiscFormat.CD, Year = 1973, DiscType = DiscType.Music },
                new Disc { DiscId = 3, ProductId = 4, Format = DiscFormat.BluRay, Year = 2020, DiscType = DiscType.Game }
            };
            context.Discs.AddRange(discs);
            context.SaveChanges();

            // Add Actors
            var actors = new List<Actor>
            {
                new Actor { ActorId = 1, Name = "Tom Hanks" },
                new Actor { ActorId = 2, Name = "Leonardo DiCaprio" }
            };
            context.Actors.AddRange(actors);
            context.SaveChanges();

            // Add Movies
            var movies = new List<Movie>
            {
                new Movie { MovieId = 1, Disc = discs[0], Duration = 148, Director = "Christopher Nolan", Actors = actors },
            };
            context.Movies.AddRange(movies);
            context.SaveChanges();

            // Add Games
            var games = new List<Game>
            {
                new Game { GameId = 1, DiscId = 3, Platform = GamePlatform.PC, Developer = "Activision" }
            };
            context.Games.AddRange(games);
            context.SaveChanges();

            // Add Musics
            var musicDiscs = new List<Music>
            {
                new Music { MusicId = 1, DiscId = 2, Artist = "Pink Floyd", MusicGenre = MusicGenre.Rock, TrackCount = 10 }
            };
            context.Music.AddRange(musicDiscs);
            context.SaveChanges();

            // Додати позичення
            var borrows = new List<Borrow>
            {
                new Borrow { BorrowId = 1, LenderId = 1, BorrowerId = 2, ProductId = 1, BorrowStartDate = DateTime.Now.AddDays(-5), BorrowEndDate = DateTime.Now.AddDays(5) },
                new Borrow { BorrowId = 2, LenderId = 2, BorrowerId = 1, ProductId = 2, BorrowStartDate = DateTime.Now.AddDays(-3), BorrowEndDate = DateTime.Now.AddDays(7)}
            };
            context.Borrows.AddRange(borrows);
            context.SaveChanges();

            // Add Orders
            var orders = new List<Rental>
            {
                new Rental { RentalId = 1, UserId = 1, ProductId = 3, RentalStartDate = DateTime.Now.AddDays(-2), RentalEndDate=DateTime.Now, PaymentAmount = 10.0M },
                new Rental { RentalId = 2, UserId = 2, ProductId = 4, RentalStartDate = DateTime.Now.AddDays(-1), RentalEndDate=DateTime.Now, PaymentAmount = 15.0M }
            };
            context.Rentals.AddRange(orders);
            context.SaveChanges();

            // *TEST CASCADE DELETION

            // Delete user:
            // var userToDelete = context.Users.FirstOrDefault(u => u.UserId == 1);
            // if (userToDelete != null)
            // {
            //     context.Users.Remove(userToDelete);
            //     context.SaveChanges();
            //     if (context.Users.FirstOrDefault(u => u.UserId == 1) == null)
            //         Console.WriteLine("User deleted.");
            // }

            // // Delete Movie Product (should cascade delete the movie, disc, and product)
            // var productToDelete = context.Products.FirstOrDefault(p => p.ProductId == movies[0].Disc.ProductId);
            // if (productToDelete != null)
            // {
            //     context.Products.Remove(productToDelete);
            //     context.SaveChanges();
            //     if(context.Movies.FirstOrDefault(m => m.MovieId == movies[0].MovieId) == null)
            //         Console.WriteLine("Movie, Disc, and Product deleted.");
            // }

            // // Check cascade deletion for Music and Game
            // productToDelete = context.Products.FirstOrDefault(p => p.ProductId == musicDiscs[0].Disc.ProductId);
            // if (productToDelete != null)
            // {
            //     context.Products.Remove(productToDelete);
            //     context.SaveChanges();
            //     if(context.Musics.FirstOrDefault(m => m.MusicId == musicDiscs[0].MusicId) == null)
            //         Console.WriteLine("Music, Disc, and Product deleted.");
            // }

            // productToDelete = context.Products.FirstOrDefault(p => p.ProductId == games[0].Disc.ProductId);
            // if (productToDelete != null)
            // {
            //     context.Products.Remove(productToDelete);
            //     context.SaveChanges();
            //     if(context.Games.FirstOrDefault(g => g.GameId == games[0].GameId) == null)
            //         Console.WriteLine("Game, Disc, and Product deleted.");
            // }

            // // Cascade deletion test for Book
            // var bookToDelete = context.Products.FirstOrDefault(p => p.ProductId == books[0].ProductId);
            // if (bookToDelete != null)
            // {
            //     context.Products.Remove(bookToDelete);
            //     context.SaveChanges();
            //     if(context.Books.FirstOrDefault(b => b.BookId == books[0].BookId) == null)
            //     Console.WriteLine("Book and Product deleted.");
            // }
        }
    }
}
