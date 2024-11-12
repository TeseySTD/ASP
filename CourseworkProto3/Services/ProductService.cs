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

    public async Task<bool> IsProductOrdered(int productId){
        return await _context.Orders
            .AnyAsync(o => o.ProductId == productId && o.Status == OrderStatus.Ordered);
    }

    public async Task<bool> IsProductOrderedByUser(int productId, string token){
        return await _context.Orders
            .AnyAsync(o => o.ProductId == productId && o.Status == OrderStatus.Ordered && o.UserId == JwtService.GetUserIdFromToken(token));
    }

    public async Task<bool> IsProductForOrder(int productId){
        return await _context.Orders
            .AnyAsync(o => o.ProductId == productId);
    }

    public async Task<bool> IsProductWaitingForOrder(int productId){
        return await _context.Orders
            .AnyAsync(o => o.ProductId == productId && o.Status == OrderStatus.Pending);
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

    public async Task<List<Borrow>> GetDebtsWithFullProductsByUserId(int userId){
        return await _context.Borrows
            .Include(b => b.Borrower)
            .Include(b => b.Product)
                .ThenInclude(p => p.Book)
                    .ThenInclude(b => b.Genre)
            .Include(b => b.Product)
                .ThenInclude(p => p.Disc)
                    .ThenInclude(d => d.Movie)
                        .ThenInclude(m => m.Genre)
            .Include(b => b.Product)
                .ThenInclude(p => p.Disc)
                    .ThenInclude(d => d.Movie)
                        .ThenInclude(m => m.Actors)
            .Include(b => b.Product)
                .ThenInclude(p => p.Disc)
                    .ThenInclude(d => d.Music)
                        .ThenInclude(m => m.Genre)
            .Include(b => b.Product)
                .ThenInclude(p => p.Disc)
                    .ThenInclude(d => d.Game)
                        .ThenInclude(m => m.Genre)
            .Include(b => b.Product)
                .ThenInclude(p => p.Owner)
            .Where(b => b.BorrowEndDate < DateTime.Now && b.BorrowerId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    // public async Task<IQueryable<Product>> GetFilteredProducts(FilterProductsRequest request)
    // {
    //     var query = _context.Products.AsQueryable();
    //     var currentUserId = JwtService.GetUserIdFromToken(request.CurrentUserToken);
    //     // Include related entities
    //     query = query
    //         .Include(p => p.Owner)
    //         .Include(p => p.Disc)
    //             .ThenInclude(d => d.Movie)
    //                 .ThenInclude(m => m.Genre)
    //         .Include(p => p.Disc)
    //             .ThenInclude(d => d.Music)
    //                 .ThenInclude(m => m.Genre)
    //         .Include(p => p.Disc)
    //             .ThenInclude(d => d.Game)
    //                 .ThenInclude(g => g.Genre)
    //         .Include(p => p.Book)
    //             .ThenInclude(b => b.Genre);

    //     // Filter by product type
    //     if (request.Type != CommonProductsType.All)
    //     {
    //         switch (request.Type)
    //         {
    //             case CommonProductsType.Book:
    //                 query = query.Where(p => p.ProductType == ProductType.Book);
    //                 break;
    //             case CommonProductsType.Movie:
    //                 query = query.Where(p => p.ProductType == ProductType.Disc && p.Disc.DiscType == DiscType.Movie);
    //                 break;
    //             case CommonProductsType.Music:
    //                 query = query.Where(p => p.ProductType == ProductType.Disc && p.Disc.DiscType == DiscType.Music);
    //                 break;
    //             case CommonProductsType.Game:
    //                 query = query.Where(p => p.ProductType == ProductType.Disc && p.Disc.DiscType == DiscType.Game);
    //                 break;
    //             case CommonProductsType.Disc:
    //                 query = query.Where(p => p.ProductType == ProductType.Disc);
    //                 break;
    //         }
    //     }

    //     // Filter by genre if specified
    //     if (!string.IsNullOrEmpty(request.Genre))
    //     {
    //         query = query.Where(p =>
    //             (p.Book != null && p.Book.Genre.Any(g => g.Name.Contains(request.Genre))) ||
    //             (p.Disc != null && p.Disc.Movie != null && p.Disc.Movie.Genre.Any(g => g.Name.Contains(request.Genre))) ||
    //             (p.Disc != null && p.Disc.Music != null && p.Disc.Music.Genre.Any(g => g.Name.Contains(request.Genre))) ||
    //             (p.Disc != null && p.Disc.Game != null && p.Disc.Game.Genre.Any(g => g.Name.Contains(request.Genre)))
    //         );
    //     }

    //     // Filter by disc format if specified
    //     if (request.Format.HasValue)
    //     {
    //         query = query.Where(p => p.Disc != null && p.Disc.Format == request.Format.Value);
    //     }

    //     // Filter by ownership status
    //     switch (request.OwnershipStatus)
    //     {
    //         case OwnershipStatus.Owned:
    //             query = query.Where(p => p.OwnerId == currentUserId);
    //             break;
    //         case OwnershipStatus.Borrowed:
    //             query.Where(p => _context.Borrows.Any(b => b.ProductId == p.ProductId) && 
    //                 _context.Borrows.Any(b => b.BorrowerId == currentUserId));
    //             break;
    //         case OwnershipStatus.Loaned:
    //             query = query.Where(p =>_context.Orders.Any(l => l.ProductId == p.ProductId && l.Status == OrderStatus.Ordered
    //                                                         && l.UserId == currentUserId));
    //             break;
    //         case OwnershipStatus.Returned:
    //             query = query.Where(p =>_context.Orders.Any(l => l.ProductId == p.ProductId && l.Status == OrderStatus.Returned
    //                                                         && l.UserId == currentUserId));
    //             break;
    //     }

    //     // Filter by added date range
    //     if (request.AddedDateRange.HasValue)
    //     {
    //         var today = DateTime.Today;
    //         var startDate = request.AddedDateRange switch
    //         {
    //             DateRange.LastWeek => today.AddDays(-7),
    //             DateRange.LastMonth => today.AddMonths(-1),
    //             _ => today
    //         };
            
    //         // query = query.Where(p => _context.ProductHistory
    //         //     .Any(h => h.ProductId == p.ProductId && 
    //         //             h.AddedDate >= startDate));
    //     }

    //     // Filter by return status
    //     if (request.ReturnStatus.HasValue)
    //     {
    //         var today = DateTime.Today;
    //         switch (request.ReturnStatus.Value)
    //         {
    //             case ReturnStatus.Overdue:
    //                 query = query.Where(p => _context.Loans
    //                     .Any(l => l.ProductId == p.ProductId && 
    //                             l.DueDate < today && 
    //                             l.ReturnDate == null));
    //                 break;
    //             case ReturnStatus.Pending:
    //                 query = query.Where(p => _context.Loans
    //                     .Any(l => l.ProductId == p.ProductId && 
    //                             l.DueDate >= today && 
    //                             l.ReturnDate == null));
    //                 break;
    //         }
    //     }

    //     // Filter by availability status
    //     if (request.AvailabilityStatus.HasValue)
    //     {
    //         switch (request.AvailabilityStatus.Value)
    //         {
    //             case AvailabilityStatus.Available:
    //                 query = query.Where(p => !_context.Loans
    //                     .Any(l => l.ProductId == p.ProductId && 
    //                             l.ReturnDate == null));
    //                 break;
    //             case AvailabilityStatus.LoanedOut:
    //                 query = query.Where(p => _context.Loans
    //                     .Any(l => l.ProductId == p.ProductId && 
    //                             l.ReturnDate == null));
    //                 break;
    //         }
    //     }

    //     return query;
    // }

}