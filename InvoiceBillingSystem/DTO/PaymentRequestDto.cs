namespace InvoiceBillingSystem.DTO
{
    public class PaymentRequestDto
    {
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }

        public Guid PaymentLinkId { get; set; }  
        public string Gateway { get; set; }  

       
    }
}
