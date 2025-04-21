using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Services
{
    public interface IJwtTokenGenerator
    {
        public string GetUserRole();
        public Guid GetUserId();
        string GenerateToken(Guid userId, string email, string role);
    }
}
