namespace InvoiceBillingSystem.DTO
{
    public class OTPVerifyDto
    {
        public Guid UserId { get; set; }
        public string OTP { get; set; }
    }
}
