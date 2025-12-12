using MassTransit;
using OrderService.Data;
using static Contracts.Events;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Consumers
{
    public class ProductReservedConsumer : IConsumer<ProductReserved>
    {
        private readonly OrderDbContext _db;
        private readonly IPublishEndpoint _publish;
        public ProductReservedConsumer(OrderDbContext db, IPublishEndpoint publish) { _db = db; _publish = publish; }

        public async Task Consume(ConsumeContext<ProductReserved> context)
        {
            var msg = context.Message;
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == msg.OrderId);
            if (order == null) return;

            if (msg.Fail)
            {
                order.Status = "Failed";
                await _db.SaveChangesAsync();
                await _publish.Publish(new OrderFailed(order.Id, order.ProductId, order.Quantity, "Simulated downstream failure"));
                return;
            }

            order.Status = "Created";
            await _db.SaveChangesAsync();
            await _publish.Publish(new OrderCompleted(order.Id));
        }
    }
}
