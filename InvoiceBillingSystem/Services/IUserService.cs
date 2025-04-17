using InvoiceBillingSystem.DTO;

namespace InvoiceBillingSystem.Services
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(UserDto userDto);
        Task<string> LoginUserAsync(LoginDto loginDto);
        Task<bool> AssignRoleAsync(Guid UserId, string role);

        Task<string> RegisterAdminAsync(AdminRegisterDto adminRegisterDto);

        Task<string> LoginAdminAsync (AdminLoginDto adminLoginDto);
    }
}
