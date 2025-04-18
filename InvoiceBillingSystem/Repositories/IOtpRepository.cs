using InvoiceBillingSystem.Models;
using static System.Net.WebRequestMethods;

namespace InvoiceBillingSystem.Repositories
{
    public interface IOtpRepository
    {
        Task SaveOTPAsync(OTP otp);
        Task<OTP> GetOTPAsync(Guid userId, string otpCode);

        Task MarkOTPAsVerifiedAsync(OTP otp);

        Task MarkExpiredUnverifiedOtpsAsVerifiedAsync();
    }
}
