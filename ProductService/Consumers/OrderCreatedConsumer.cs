using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using Contracts;
using System.Threading.Tasks;
using System;
using static Contracts.Events;

namespace ProductService.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly ProductDbContext _db;
        public OrderCreatedConsumer(ProductDbContext db) => _db = db;

        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            var msg = context.Message;
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == msg.ProductId);
            if (product == null)
            {
                await context.Publish(new OrderFailed(msg.OrderId, msg.ProductId, msg.Quantity, "Product not found"));
                return;
            }

            if (product.Stock < msg.Quantity)
            {
                await context.Publish(new OrderFailed(msg.OrderId, msg.ProductId, msg.Quantity, "Insufficient stock"));
                return;
            }

            // Reserve (simple transactional update)
            product.Stock -= msg.Quantity;
            await _db.SaveChangesAsync();
            var productReserved = new ProductReserved(msg.OrderId, msg.ProductId, msg.Quantity, false);
            await context.Publish(productReserved);
        }
    }
}
