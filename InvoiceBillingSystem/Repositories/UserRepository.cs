using InvoiceBillingSystem.Data;
using InvoiceBillingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBillingSystem.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user); 
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User?> GetUserByIdAsync(Guid UserId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
        }

        public async Task<Admin> CreateAdminAsync(Admin admin)
        {
            _context.Admin.Add(admin);
            await _context.SaveChangesAsync();
            return admin;
        }

        public async Task<Admin?> GetAdminByEmailAsync(string AdminEmail)
        {
            return await _context.Admin.FirstOrDefaultAsync(e=>e.Email == AdminEmail);
        }

        public async Task<PasswordResetToken> GetResetTokenAsync(string email, string token)
        {
            var user = await GetUserByEmailAsync(email);
            return await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == token && t.UserId == user.Id && !t.IsUsed && t.Expiry > DateTime.UtcNow);
        }

        public async Task CreateResetTokenAsync(PasswordResetToken token)
        {
            _context.PasswordResetTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task MarkTokenAsUsedAsync(PasswordResetToken token)
        {
            token.IsUsed = true;
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePasswordAsync(User user, string newHashedPassword)
        {
            user.PasswordHash = newHashedPassword;
            await _context.SaveChangesAsync();
        }
    }
}
