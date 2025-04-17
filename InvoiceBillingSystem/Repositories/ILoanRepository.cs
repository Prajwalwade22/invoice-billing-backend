using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public interface ILoanRepository
    {
        Task AddLoanApplicationAsync(LoanApplication loanApplication);
        Task<LoanApplication> ApplyForLoanAsync(LoanApplication loanApplication);
        Task<IEnumerable<LoanApplication>> GetLoanApplicationsByUserIdAsync(Guid userId);
        Task<bool> UpdateLoanStatusAsync(Guid loanId, string status);
        Task<int> GetLoanApplicationsCountByUserIdAsync(Guid userId);
    }
}
