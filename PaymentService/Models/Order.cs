namespace PaymentService.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
