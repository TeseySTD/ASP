using Library.Data;
using Library.Data.Repo;
using System.Threading.Tasks;
using System.Linq;
using Library.Models.Entities;
using Library.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{ 
    public class OrderService
    {
        private readonly ProductRepository _productRepository;
        private readonly LibraryContext _context;
        private readonly ExternalSourceService _externalSourceService;

        private readonly Random _random = new Random();

        public OrderService(ProductRepository productRepository,
                            LibraryContext context,
                            ExternalSourceService externalSourceService)
        {
            _context = context;
            _productRepository = productRepository;
            _externalSourceService = externalSourceService;
        }

        public async Task<Order?> GetPendingOrder(int productId){
            return await _context.Orders.FirstOrDefaultAsync(o => o.ProductId == productId  && o.Status == OrderStatus.Pending);
        }

        public async Task<Order?> GetOrderedOrder(int productId){
            return await  _context.Orders.FirstOrDefaultAsync(o => o.ProductId == productId  && o.Status == OrderStatus.Ordered);
        }

        public async Task<List<Order>> GetOrders(){
            return await _context.Orders
                                    .Include(o => o.Product)
                                    .Include(o => o.User)
                                    .AsNoTracking()
                                    .ToListAsync();
        }   

        public async Task CheckOrdersAsync()
        {
            var pendingOrders = _context.Orders
            .Where(o => o.Status == OrderStatus.Pending && o.Product != null)
            .ToList();

            int ordersToGenerate = 6 - pendingOrders.Count;

            if (ordersToGenerate > 0)
            {
                for (int i = 0; i < ordersToGenerate; i++)
                {
                    await GenerateOrder();
                }
            }
        }

        public async Task MakeOrder(OrderDTO dto){
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.ProductId == dto.ProductId  && o.Status == OrderStatus.Pending);
            
            if (order == null)  
                return;

            order.Status = OrderStatus.Ordered;
            order.Product = _context.Products.FirstOrDefault(p => p.ProductId == dto.ProductId);
            order.StartDate = dto.StartDate;
            order.EndDate = dto.EndDate;
            order.UserId = dto.UserId;
            await _context.SaveChangesAsync();
        }

        public async Task ReturnOrder(int productId){
            var order = await _context.Orders.FirstOrDefaultAsync(
                    o => o.ProductId == productId && o.Status == OrderStatus.Ordered);
            
            if (order == null)  
                return;
            
            order.Status = OrderStatus.Returned;
            await _context.SaveChangesAsync();

            var newOrder = new Order
            {
                Status = OrderStatus.Pending,
                ProductId = order.ProductId,
                PaymentAmount = order.PaymentAmount,
            };
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();
        }

        private async Task GenerateOrder(){
            var newProduct = await _externalSourceService.GetProduct();
            var newOrder = new Order
            {
                Status = OrderStatus.Pending,
                Product = newProduct, 
                PaymentAmount = GetRandomDecimal(1m, 10000m),
            };
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();
        }

        private decimal GetRandomDecimal(decimal minValue, decimal maxValue)
        {
            Random random = new Random();
            double range = (double)(maxValue - minValue);
            double sample = random.NextDouble();
            return minValue + (decimal)(sample * range);
        }

    }
}
