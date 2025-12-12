using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using static Contracts.Events;

namespace OrderService.Consumers
{
    public class OrderFailedConsumer : IConsumer<OrderFailed>
    {
        private readonly OrderDbContext _db;
        public OrderFailedConsumer(OrderDbContext db) => _db = db;

        public async Task Consume(ConsumeContext<OrderFailed> context)
        {
            var msg = context.Message;
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == msg.OrderId);
            if (order != null)
            {
                order.Status = "Failed";
                await _db.SaveChangesAsync();
            }
        }
    }
}
