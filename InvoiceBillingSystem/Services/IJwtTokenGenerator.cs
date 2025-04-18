using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Services
{
    public interface IJwtTokenGenerator
    {

        public Guid GetUserId();
        string GenerateToken(Guid userId, string email, string role);
    }
}
