using MassTransit;
using static Contracts.Events;

namespace EmailService.Consumers
{
    public class OrderCompletedConsumer : IConsumer<OrderCompleted>
    {
        private readonly ILogger<OrderCompletedConsumer> _logger;
        public OrderCompletedConsumer(ILogger<OrderCompletedConsumer> logger) => _logger = logger;

        public Task Consume(ConsumeContext<OrderCompleted> context)
        {
            _logger.LogInformation("EMAIL: Order {OrderId} created successfully.", context.Message.OrderId);
            return Task.CompletedTask;
        }
    }
}
