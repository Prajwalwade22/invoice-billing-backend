namespace InvoiceBillingSystem.DTO
{
    public class RefundRequestDto
    {
        public Guid InvoiceId { get; set; }
        public decimal RefundAmount { get; set; }
        public string Reason { get; set; }
    }
}
