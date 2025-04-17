using InvoiceBillingSystem.DTO;

namespace InvoiceBillingSystem.Services
{
    public interface IForgotPasswordService
    {
        Task<string> GenerateResetTokenAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    }
}
