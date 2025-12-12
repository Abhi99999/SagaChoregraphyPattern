using MassTransit;
using static Contracts.Events;

namespace EmailService.Consumers
{
    public class OrderFailedConsumer : IConsumer<OrderFailed>
    {
        private readonly ILogger<OrderFailedConsumer> _logger;
        public OrderFailedConsumer(ILogger<OrderFailedConsumer> logger) => _logger = logger;

        public Task Consume(ConsumeContext<OrderFailed> context)
        {
            _logger.LogInformation("EMAIL: Order {OrderId} failed. Reason: {Reason}", context.Message.OrderId, context.Message.Reason);
            return Task.CompletedTask;
        }
    }
}
