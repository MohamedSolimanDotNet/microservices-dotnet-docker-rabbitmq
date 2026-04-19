namespace PaymentService.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
