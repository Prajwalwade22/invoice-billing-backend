using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public interface IInvestmentRepository
    {
        Task AddTransactionAsync(InvestmentTransaction transaction);
        Task<User?> GetUserByIdAsync(Guid UserId);
    }
}
