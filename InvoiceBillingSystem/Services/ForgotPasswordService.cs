using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;

namespace InvoiceBillingSystem.Services
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly IUserRepository _repo;

        public ForgotPasswordService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> GenerateResetTokenAsync(string email)
        {
            var user = await _repo.GetUserByEmailAsync(email);
            if (user == null) return null;

            var token = Guid.NewGuid().ToString();
            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                Expiry = DateTime.UtcNow.AddMinutes(30),
                IsUsed = false
            };
            
            await _repo.CreateResetTokenAsync(resetToken);
            return token;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var token = await _repo.GetResetTokenAsync(dto.Email, dto.Token);
            if (token == null) return false;

            var user = await _repo.GetUserByEmailAsync(dto.Email);
            if (user == null) return false;

            // Hash new password
            var newHashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _repo.UpdatePasswordAsync(user, newHashedPassword);
            await _repo.MarkTokenAsUsedAsync(token);

            return true;
        }
    }
}
