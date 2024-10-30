using Library.Data;
using Library.Data.Repo;
using Library.Models.DTO;
using Library.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Services;

public class ProductService{

    private readonly LibraryContext _context;
    private readonly ProductRepository _productRepository;

    public ProductService(LibraryContext context, ProductRepository productRepository){
        _context = context;
        _productRepository = productRepository;
    }

    public async Task BorrowProduct(BorrowDto dto){
        if(!await _productRepository.ProductExists(dto.ProductId)){
            return;
        }

        var borrow = new Borrow
        {
            BorrowerId = dto.BorrowerId,
            LenderId = dto.LenderId,
            ProductId = dto.ProductId,
            BorrowStartDate = dto.BorrowStartDate,
            BorrowEndDate = dto.BorrowEndDate,
        };
        _context.Borrows.Add(borrow);
        
        await _context.SaveChangesAsync();
    }

    public async Task ReturnProduct(int productId){
        var borrow = await GetBorrowByProductId(productId);
        if(borrow == null){
            return;
        }
        _context.Borrows.Remove(borrow);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsProductBorrowed(int productId){
        return await _context.Borrows
            .AnyAsync(b => b.ProductId == productId);
    }

    public async Task<bool> IsProductBorrowedByUser(int productId, string token){
        return await _context.Borrows
            .AnyAsync(b => b.ProductId == productId && b.BorrowerId == JwtService.GetUserIdFromToken(token));
    }

    public async Task<bool> IsUserOwner(int productId, string token)
    {
        var userId = JwtService.GetUserIdFromToken(token);

        if (userId == null)
        {
            return false;
        }

        var product = await _productRepository.GetById(productId);

        return product?.OwnerId == userId.Value;
    }

    public async Task<Borrow?> GetBorrowByProductId(int productId){
        return await _context.Borrows
            .Include(b => b.Lender)
            .Include(b => b.Borrower)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.ProductId == productId);
    }

}