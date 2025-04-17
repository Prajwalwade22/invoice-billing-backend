namespace InvoiceBillingSystem.Models
{
    public class OTP
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Code { get; set; } 
        public DateTime ExpiryTime { get; set; } 
        public bool IsVerified { get; set; } = false;
    }
}
