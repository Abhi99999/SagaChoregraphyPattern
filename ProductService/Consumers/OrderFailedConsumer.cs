using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using Contracts;
using System.Threading.Tasks;
using static Contracts.Events;

namespace ProductService.Consumers
{
    public class OrderFailedConsumer : IConsumer<OrderFailed>
    {
        private readonly ProductDbContext _db;
        public OrderFailedConsumer(ProductDbContext db) => _db = db;

        public async Task Consume(ConsumeContext<OrderFailed> context)
        {
            var msg = context.Message;
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == msg.ProductId);
            if (product != null)
            {
                product.Stock += msg.Quantity; // revert reservation
                await _db.SaveChangesAsync();
            }
        }
    }
}
