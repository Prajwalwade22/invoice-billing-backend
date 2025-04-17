namespace InvoiceBillingSystem.Models
{
    public class AuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Action { get; set; } // E.g "Invoice Updated"
        public string PerformedBy { get; set; } 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Details { get; set; } 
    }
}
