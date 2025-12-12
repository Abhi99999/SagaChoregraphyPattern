namespace OrderService.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Created, Failed
        public bool Fail { get; set; } // used to simulate later failure
    }
}
