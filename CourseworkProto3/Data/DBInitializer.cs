using Library.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public static class DBInitializer
{
    public static void ReCreate(bool recreate = true)
    {
        using (LibraryContext context = new LibraryContext())
        {
            // Recreate the database
            if(recreate)
                context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }

    public static void Test(){
        using (LibraryContext context = new LibraryContext()){
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

    public static void SeedDataInCreatedDB()
    {
        using (LibraryContext context = new LibraryContext()){ 
            if(!context.Users.Any()){
                // Add Users
                var users = new List<User>
                {
                    new User { UserId = 1, Login = "root", Email = "root@gmail.com", Password = "pass", Gender = Gender.Male, Role = Roles.Owner },
                    new User { UserId = 2, Login = "Jane Smith", Email = "jane@gmail.com", Password = "pass", Gender = Gender.Female, Role = Roles.Administrator },
                    new User { UserId = 3, Login = "John Doe", Email = "john@gmail.com", Password = "pass", Gender = Gender.Male, Role = Roles.Operator },
                    new User { UserId = 4, Login = "Jane Doe", Email = "jane2@gmail.com", Password = "pass", Gender = Gender.Female, Role = Roles.Default }
                };
                context.Users.AddRange(users);
                context.SaveChanges();

                // Add Products
                var products = new List<Product>
                {
                    new Product { ProductId = 1, Title = "Великий Гетсбі", ProductType = ProductType.Book},
                    new Product { ProductId = 2, Title = "Inception", ProductType = ProductType.Disc},
                    new Product { ProductId = 3, Title = "The Dark Side of the Moon", ProductType = ProductType.Disc},
                    new Product { ProductId = 4, Title = "Call of Duty", ProductType = ProductType.Disc},
                    new Product { ProductId = 5, Title = "Zelda", ProductType = ProductType.Disc},
                    new Product { ProductId = 6, Title = "The Lord of the Rings", ProductType = ProductType.Book}
                };
                products[0].Owner = users[0];
                products[1].Owner = users[0];
                products[2].Owner = users[0];
                products[3].Owner = users[1];
                products[4].Owner = users[2];
                products[5].Owner = users[3];

                context.Products.AddRange(products);
                context.SaveChanges();

                // Add Discs
                var discs = new List<Disc>
                {
                    new Disc { DiscId = 1, ProductId = 2, Format = DiscFormat.BluRay, Year = 2010, DiscType = DiscType.Movie },
                    new Disc { DiscId = 2, ProductId = 3, Format = DiscFormat.CD, Year = 1973, DiscType = DiscType.Music },
                    new Disc { DiscId = 3, ProductId = 4, Format = DiscFormat.BluRay, Year = 2020, DiscType = DiscType.Game },
                    new Disc { DiscId = 4, ProductId = 5, Format = DiscFormat.CD, Year = 1986, DiscType = DiscType.Music }
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

                //Add book genres
                var bookGenres = new List<BookGenre>{
                    new BookGenre { GenreId = 1, Name = "Фантастика" },
                    new BookGenre { GenreId = 2, Name = "Фентезі" },
                    new BookGenre { GenreId = 3, Name = "Хоррор" }
                };
                context.BookGenres.AddRange(bookGenres);
                context.SaveChanges();

                // Add game genres
                var gameGenres = new List<GameGenre>{
                    new GameGenre { GenreId = 1, Name = "Екшн" },
                };
                context.GameGenres.AddRange(gameGenres);
                context.SaveChanges();

                //Add Music genres
                var musicGenres = new List<MusicGenre>{
                    new MusicGenre { GenreId = 1, Name = "Рок" },
                };
                context.MusicGenres.AddRange(musicGenres);
                context.SaveChanges();

                //Add movie genres
                var movieGenres = new List<MovieGenre>{
                    new MovieGenre { GenreId = 1, Name = "Драма" },
                    new MovieGenre { GenreId = 2, Name = "Екшн" }
                };

                // Add Books
                var books = new List<Book>
                {
                    new Book { BookId = 1, ProductId = 1, Author = "F. Scott Fitzgerald", Genre = bookGenres, PublicationYear = 1925 },
                    new Book { BookId = 2, ProductId = 6, Author = "J. R. R. Tolkien", Genre = bookGenres, PublicationYear = 1954 }
                };
                context.Books.AddRange(books);
                context.SaveChanges();

                // Add Movies
                var movies = new List<Movie>
                {
                    new Movie { MovieId = 1, Disc = discs[0], Duration = 148, Director = "Christopher Nolan", Genre = movieGenres, Actors = actors },
                };
                context.Movies.AddRange(movies);
                context.SaveChanges();

                // Add Games
                var games = new List<Game>
                {
                    new Game { GameId = 1, DiscId = 3, Developer = "Activision", Publisher = "Activision", Genre = gameGenres },
                    new Game { GameId = 2, DiscId = 4, Developer = "Nintendo", Publisher = "Nintendo", Genre = gameGenres }
                };
                context.Games.AddRange(games);
                context.SaveChanges();

                // Add Musics
                var musicDiscs = new List<Music>
                {
                    new Music { MusicId = 1, DiscId = 2, Artist = "Pink Floyd", Genre = musicGenres, TrackCount = 10 }
                };
                context.Music.AddRange(musicDiscs);
                context.SaveChanges();

                // Додати позичення
                var borrows = new List<Borrow>
                {
                    new Borrow { BorrowId = 1, LenderId = 1, BorrowerId = 2, ProductId = 1, BorrowStartDate = DateTime.Now.AddDays(-5), BorrowEndDate = DateTime.Now },
                    new Borrow { BorrowId = 2, LenderId = 1, BorrowerId = 2, ProductId = 2, BorrowStartDate = DateTime.Now.AddDays(-3), BorrowEndDate = DateTime.Now.AddDays(7) },
                    // new Borrow { BorrowId = 3, LenderId = 3, BorrowerId = 1, ProductId = 3, BorrowStartDate = DateTime.Now.AddDays(-2), BorrowEndDate = DateTime.Now.AddDays(-1)}
                    
                };
                context.Borrows.AddRange(borrows);
                context.SaveChanges();

                // Add Orders
                var orders = new List<Order>
                {
                    new Order{ OrderId = 1, ProductId = 3, UserId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) , PaymentAmount = 5.00m, Status = OrderStatus.Ordered },
                };
                context.Orders.AddRange(orders);
                context.SaveChanges();
            }

        }
    }
}

