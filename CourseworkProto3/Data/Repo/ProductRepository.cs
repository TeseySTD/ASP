using System;
using Library.Models.DTO;
using Library.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repo;

public class ProductRepository
{
    private readonly LibraryContext _context;

    public ProductRepository(LibraryContext context)
    {
        _context = context;
    }

    public void Add(Product product){
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public async Task<Product?> Get(){
        return await _context.Products
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
    }

    public async Task<Product?> GetById(int id){
        return await _context.Products
                    .Include(p => p.Book)
                        .ThenInclude(b => b.Genre)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Movie)
                            .ThenInclude(m => m.Genre)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Movie)
                            .ThenInclude(m => m.Actors)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Music)
                            .ThenInclude(m => m.Genre)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Game)
                            .ThenInclude(m => m.Genre)
                    .Include(p => p.Owner)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    private async Task<Product?> GetByIdWithTracking(int id){
        return await _context.Products
                    .Include(p => p.Book)
                        .ThenInclude(b => b.Genre)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Movie)
                            .ThenInclude(m => m.Genre)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Movie)
                            .ThenInclude(m => m.Actors)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Music)
                            .ThenInclude(m => m.Genre)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Game)
                            .ThenInclude(m => m.Genre)
                    .Include(p => p.Owner)
                    .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<List<Product>> GetAll(){
        return await _context.Products
                    .Include(p => p.Book)
                        .ThenInclude(b => b.Genre)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Movie)
                            .ThenInclude(m => m.Genre)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Movie)
                            .ThenInclude(m => m.Actors)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Music)
                            .ThenInclude(m => m.Genre)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Game)
                            .ThenInclude(m => m.Genre)
                    .Include(p => p.Owner)
                    .AsNoTracking()
                    .ToListAsync();
    }


    public async Task Update(Product product){
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task Update(ProductDto dto)
    {
        var product = await GetByIdWithTracking(dto.ProductId);

        if (product == null)
        {
            return;
        }

        product.Title = dto.Title;

        if (dto.Book != null && product.Book != null)
        {
            product.Book.Author = dto.Book.Author;
            // Оновлюємо жанри книги
            var updatedBookGenres = dto.Book.Genre.Split(",").Select(g => g.Trim()).ToList();
            var currentBookGenres = product.Book.Genre.Select(bg => bg.Name).ToList();

            // 1. Додаємо нові жанри, яких немає в книзі
            foreach (var genreName in updatedBookGenres)
            {
                if (!currentBookGenres.Contains(genreName))
                {
                    var genre = await _context.BookGenres.FirstOrDefaultAsync(g => g.Name == genreName);

                    if (genre == null)
                    {
                        genre = new BookGenre { Name = genreName };
                        _context.BookGenres.Add(genre);
                        await _context.SaveChangesAsync(); // Зберігаємо новий жанр у базу
                    }

                    product.Book.Genre.Add(genre);
                }
            }

            // 2. Видаляємо жанри, яких немає в оновленому списку
            var bookGenresToRemove = product.Book.Genre
                .Where(bg => !updatedBookGenres.Contains(bg.Name))
                .ToList();

            foreach (var genreToRemove in bookGenresToRemove)
            {
                product.Book.Genre.Remove(genreToRemove);
            }

            product.Book.PublicationYear = dto.Book.PublicationYear;
        }

        if (dto.Disc != null && product.Disc != null)
        {
            product.Disc.Format = dto.Disc.Format;
            product.Disc.Year = dto.Disc.Year;

            if (dto.Disc.Movie != null && product.Disc.Movie != null)
            {
                product.Disc.Movie.Director = dto.Disc.Movie.Director;
                product.Disc.Movie.Duration = dto.Disc.Movie.Duration;

                // Оновлюємо жанри фільму
                var updatedMovieGenres = dto.Disc.Movie.Genre.Split(",").Select(g => g.Trim()).ToList();
                var currentMovieGenres = product.Disc.Movie.Genre.Select(mg => mg.Name).ToList();

                foreach (var genreName in updatedMovieGenres)
                {
                    if (!currentMovieGenres.Contains(genreName))
                    {
                        var genre = await _context.MovieGenres.FirstOrDefaultAsync(g => g.Name == genreName);
                        if (genre == null)
                        {
                            genre = new MovieGenre { Name = genreName };
                            _context.MovieGenres.Add(genre);
                            await _context.SaveChangesAsync();
                        }
                        product.Disc.Movie.Genre.Add(genre);
                    }
                }

                var movieGenresToRemove = product.Disc.Movie.Genre
                    .Where(mg => !updatedMovieGenres.Contains(mg.Name))
                    .ToList();

                foreach (var genreToRemove in movieGenresToRemove)
                {
                    product.Disc.Movie.Genre.Remove(genreToRemove);
                }

                // Оновлення акторів
                var updatedActors = dto.Disc.Movie.Actors.Split(",").Select(a => a.Trim()).ToList();
                var currentActors = product.Disc.Movie.Actors.Select(a => a.Name).ToList();

                foreach (var actorName in updatedActors)
                {
                    if (!currentActors.Contains(actorName))
                    {
                        var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == actorName);
                        if (actor == null)
                        {
                            actor = new Actor { Name = actorName };
                            _context.Actors.Add(actor);
                            await _context.SaveChangesAsync();
                        }
                        product.Disc.Movie.Actors.Add(actor);
                    }
                }

                var actorsToRemove = product.Disc.Movie.Actors
                    .Where(a => !updatedActors.Contains(a.Name))
                    .ToList();

                foreach (var actorToRemove in actorsToRemove)
                {
                    product.Disc.Movie.Actors.Remove(actorToRemove);
                }

                await _context.SaveChangesAsync();
            }

            if (dto.Disc.Music != null && product.Disc.Music != null)
            {
                product.Disc.Music.Artist = dto.Disc.Music.Artist;

                // Оновлюємо жанри музики
                var updatedMusicGenres = dto.Disc.Music.Genre.Split(",").Select(g => g.Trim()).ToList();
                var currentMusicGenres = product.Disc.Music.Genre.Select(mg => mg.Name).ToList();

                foreach (var genreName in updatedMusicGenres)
                {
                    if (!currentMusicGenres.Contains(genreName))
                    {
                        var genre = await _context.MusicGenres.FirstOrDefaultAsync(g => g.Name == genreName);
                        if (genre == null)
                        {
                            genre = new MusicGenre { Name = genreName };
                            _context.MusicGenres.Add(genre);
                            await _context.SaveChangesAsync();
                        }
                        product.Disc.Music.Genre.Add(genre);
                    }
                }

                var musicGenresToRemove = product.Disc.Music.Genre
                    .Where(mg => !updatedMusicGenres.Contains(mg.Name))
                    .ToList();

                foreach (var genreToRemove in musicGenresToRemove)
                {
                    product.Disc.Music.Genre.Remove(genreToRemove);
                }

                await _context.SaveChangesAsync();
            }

            if (dto.Disc.Game != null && product.Disc.Game != null)
            {
                product.Disc.Game.Developer = dto.Disc.Game.Developer;
                product.Disc.Game.Publisher = dto.Disc.Game.Publisher;

                // Оновлюємо жанри гри
                var updatedGameGenres = dto.Disc.Game.Genre.Split(",").Select(g => g.Trim()).ToList();
                var currentGameGenres = product.Disc.Game.Genre.Select(gg => gg.Name).ToList();

                foreach (var genreName in updatedGameGenres)
                {
                    if (!currentGameGenres.Contains(genreName))
                    {
                        var genre = await _context.GameGenres.FirstOrDefaultAsync(g => g.Name == genreName);
                        if (genre == null)
                        {
                            genre = new GameGenre { Name = genreName };
                            _context.GameGenres.Add(genre);
                            await _context.SaveChangesAsync();
                        }
                        product.Disc.Game.Genre.Add(genre);
                    }
                }

                var gameGenresToRemove = product.Disc.Game.Genre
                    .Where(gg => !updatedGameGenres.Contains(gg.Name))
                    .ToList();

                foreach (var genreToRemove in gameGenresToRemove)
                {
                    product.Disc.Game.Genre.Remove(genreToRemove);
                }

                await _context.SaveChangesAsync();
            }
        }

        await _context.SaveChangesAsync();
    }


    public async Task DeleteById(int id){
        var product = await _context.Products.FindAsync(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }


    public async Task<bool> ProductExists(int id){
        return await _context.Products.AnyAsync(p => p.ProductId == id);
    }
    
}
