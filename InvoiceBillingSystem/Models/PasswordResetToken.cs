namespace InvoiceBillingSystem.Models
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
        public bool IsUsed { get; set; }
    }
}
