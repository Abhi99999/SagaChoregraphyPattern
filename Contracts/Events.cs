namespace Contracts
{
    public class Events
    {
        // Published by OrderService when an order is created
        public record OrderCreated(Guid OrderId, Guid ProductId, int Quantity, bool Fail);

        // Published by ProductService when product is reserved
        public record ProductReserved(Guid OrderId, Guid ProductId, int Quantity, bool Fail);

        // Published whenever an order fails (either product reservation failed or order later failed)
        public record OrderFailed(Guid OrderId, Guid ProductId, int Quantity, string Reason);

        // Published when order completes successfully
        public record OrderCompleted(Guid OrderId);
    }
}
