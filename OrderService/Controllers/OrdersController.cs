using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Models;
using static Contracts.Events;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _db;
        private readonly IPublishEndpoint _publish;

        public OrdersController(OrderDbContext db, IPublishEndpoint publish)
        {
            _db = db;
            _publish = publish;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Status = "Pending",
                Fail = dto.Fail
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            await _publish.Publish(new OrderCreated(order.Id, order.ProductId, order.Quantity, order.Fail));

            return Accepted(new { orderId = order.Id });
        }

        public record CreateOrderDto(Guid ProductId, int Quantity, bool Fail);
    }
}
