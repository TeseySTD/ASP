using System;
using Library.Models.DTO;
using Library.Models.Entities;
using Library.Services;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repo;

public class ProductRepository
{
    private readonly LibraryContext _context;
    private readonly ManyToManyService _manyToManyService;

    public ProductRepository(LibraryContext context, ManyToManyService genreService)
    {
        _context = context;
        _manyToManyService = genreService;
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
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.ProductId == id);
    }


    public async Task<Product?> GetByIdWitTracking(int id){
        return await _context.Products
                        .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<Product?> GetFullById(int id){
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

    public async Task<Product?> GetFullByIdWithTracking(int id){
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

    public async Task<List<Product>> GetFull(){
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
        var product = await GetFullByIdWithTracking(dto.ProductId);

        if (product == null)
        {
            return;
        }

        product.Title = dto.Title;

        if (dto.Book != null && product.Book != null)
        {
            product.Book.Author = dto.Book.Author;
            product.Book.Genre = await _manyToManyService.ProcessBookGenresAsync(dto.Book.Genre);
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

                product.Disc.Movie.Genre = await _manyToManyService.ProcessMovieGenresAsync(dto.Disc.Movie.Genre);

                product.Disc.Movie.Actors = await _manyToManyService.ProcessActorsAsync(dto.Disc.Movie.Actors);

            }

            if (dto.Disc.Music != null && product.Disc.Music != null)
            {
                product.Disc.Music.Artist = dto.Disc.Music.Artist;

                product.Disc.Music.Genre = await _manyToManyService.ProcessMusicGenresAsync(dto.Disc.Music.Genre);
            }

            if (dto.Disc.Game != null && product.Disc.Game != null)
            {
                product.Disc.Game.Developer = dto.Disc.Game.Developer;
                product.Disc.Game.Publisher = dto.Disc.Game.Publisher;

                product.Disc.Game.Genre = await _manyToManyService.ProcessGameGenresAsync(dto.Disc.Game.Genre);
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task AddBook(ProductDto dto){
        var product = new Product
        {
            Title = dto.Title,
            ProductType = ProductType.Book, 
            OwnerId = (int)dto.OwnerId
        };
        
        var book = new Book
        {
            Author = dto.Book.Author,
            PublicationYear = dto.Book.PublicationYear,
            Genre = await _manyToManyService.ProcessBookGenresAsync(dto.Book.Genre),
        };

        product.Book = book;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task AddMusic(ProductDto dto){
        var product = new Product
        {
            Title = dto.Title,
            ProductType = ProductType.Disc, 
            OwnerId = (int)dto.OwnerId,
            Disc = new Disc
            {
                Format = dto.Disc.Format,
                DiscType = DiscType.Music,
                Year = dto.Disc.Year,
                Music = new Music{
                    Artist = dto.Disc.Music.Artist,
                    Genre = await _manyToManyService.ProcessMusicGenresAsync(dto.Disc.Music.Genre),
                    TrackCount = dto.Disc.Music.TrackCount
                }
            }
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task AddGame(ProductDto dto){
        var product = new Product
        {
            Title = dto.Title,
            ProductType = ProductType.Disc, 
            OwnerId = (int)dto.OwnerId,
            Disc = new Disc
            {
                Format = dto.Disc.Format,
                DiscType = DiscType.Game,
                Year = dto.Disc.Year,
                Game = new Game{
                    Developer = dto.Disc.Game.Developer,
                    Publisher = dto.Disc.Game.Publisher,
                    Genre = await _manyToManyService.ProcessGameGenresAsync(dto.Disc.Game.Genre)
                }
            }
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task AddMovie(ProductDto dto){        
        var product = new Product
        {
            Title = dto.Title,
            ProductType = ProductType.Disc, 
            OwnerId = (int)dto.OwnerId,
            Disc = new Disc
            {
                Format = dto.Disc.Format,
                DiscType = DiscType.Movie,
                Year = dto.Disc.Year,
                Movie = new Movie
                {
                    Director = dto.Disc.Movie.Director,
                    Genre = await _manyToManyService.ProcessMovieGenresAsync(dto.Disc.Movie.Genre),
                    Duration = dto.Disc.Movie.Duration,
                    Actors = await _manyToManyService.ProcessActorsAsync(dto.Disc.Movie.Actors)
                }
            }
        };

        _context.Products.Add(product);
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
