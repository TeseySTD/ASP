using System;
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

    public async Task<List<Product>> GetAll(){
        return await _context.Products
                    .Include(p => p.Book)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Movie)
                            .ThenInclude(m => m.Actors)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Music)
                    .Include(p => p.Disc)
                        .ThenInclude(d => d.Game)
                    .Include(p => p.Owner)
                    .AsNoTracking()
                    .ToListAsync();
    }


    public async Task Update(Product product){
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteById(int id){
        var product = await _context.Products.FindAsync(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    
}
