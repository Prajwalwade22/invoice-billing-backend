namespace InvoiceBillingSystem.Services
{
    public interface IOtpService
    {
        Task<bool> GenerateAndSendOTPAsync(Guid userId, string email, string phone);
        Task<bool> VerifyOTPAsync(Guid userId, string otpCode);
    }
}
