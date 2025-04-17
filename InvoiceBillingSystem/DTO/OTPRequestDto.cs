namespace InvoiceBillingSystem.DTO
{
    public class OTPRequestDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
