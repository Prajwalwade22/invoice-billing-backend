namespace InvoiceBillingSystem.Models
{
    public class CreditNote
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }  //refunded amount
        public string Reason { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRefunded { get; set; } = false;

        public Invoice Invoice { get; set; }
    }
}
