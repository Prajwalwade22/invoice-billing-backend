namespace InvoiceBillingSystem.Models
{
    public class PaymentLink
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid InvoiceId { get; set; }
        public string UniqueLink { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsUsed { get; set; } = false; 

        public Invoice Invoice { get; set; } 

        public DateTime CreatedAt { get; set; }
    } 

}
