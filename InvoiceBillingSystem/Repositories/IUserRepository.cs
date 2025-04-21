using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<User?> GetUserByIdAsync(Guid UserId);

        Task<Admin> CreateAdminAsync(Admin admin);

        Task<Admin?> GetAdminByEmailAsync(string AdminEmail);


        Task<PasswordResetToken> GetResetTokenAsync(string email, string token);
        Task CreateResetTokenAsync(PasswordResetToken token);
        Task MarkTokenAsUsedAsync(PasswordResetToken token);
        Task UpdatePasswordAsync(User user, string newHashedPassword);

    }
}
