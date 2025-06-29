namespace PaymentsMS.Core.DTOs
{
    public class PaymentMethodDto
    {
        public string Id { get; set; } 
        public string Brand { get; set; }
        public string Last4 { get; set; }
        public long ExpMonth { get; set; }
        public long ExpYear { get; set; }
    }
}
