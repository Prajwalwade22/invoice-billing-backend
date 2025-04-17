using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string email, string role);
    }
}
