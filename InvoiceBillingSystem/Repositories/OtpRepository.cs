using InvoiceBillingSystem.Data;
using InvoiceBillingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBillingSystem.Repositories
{
    public class OtpRepository:IOtpRepository
    {
        private readonly ApplicationDbContext _context;

        public OtpRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveOTPAsync(OTP otp)
        {
            _context.OTP.AddAsync(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<OTP> GetOTPAsync(Guid userId, string otpCode)
        {
            return await _context.OTP
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Code == otpCode && o.ExpiryTime > DateTime.UtcNow && !o.IsVerified);
        }

        public async Task MarkOTPAsVerifiedAsync(OTP otp)
        {
            otp.IsVerified = true;
            await _context.SaveChangesAsync();
        }
    }
}
