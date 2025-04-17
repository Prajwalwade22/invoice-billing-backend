using InvoiceBillingSystem.Data;
using InvoiceBillingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBillingSystem.Repositories
{
    public class LoanRepository:ILoanRepository
    {
        private readonly ApplicationDbContext _context;

        public LoanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddLoanApplicationAsync(LoanApplication loanApplication)
        {
            await _context.LoanApplications.AddAsync(loanApplication);
            await _context.SaveChangesAsync();
        }

        public async Task<LoanApplication> ApplyForLoanAsync(LoanApplication loanApplication)
        {
            loanApplication.EMI = CalculateEMI(loanApplication.LoanAmount, (decimal)loanApplication.InterestRate, loanApplication.LoanTermMonths);

            await _context.LoanApplications.AddAsync(loanApplication);
            await _context.SaveChangesAsync();
            return loanApplication;
        }

        public async Task<IEnumerable<LoanApplication>> GetLoanApplicationsByUserIdAsync(Guid userId)
        {
            return await _context.LoanApplications
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> UpdateLoanStatusAsync(Guid loanId, string status)
        {
            var loan = await _context.LoanApplications.FindAsync(loanId);
            if (loan != null)
            {
                loan.Status = status;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private decimal CalculateEMI(decimal loanAmount, decimal interestRate, int loanTermMonths)
        {
            if (interestRate == 0 || loanTermMonths == 0)
            {
                return loanAmount / loanTermMonths; 
            }

            decimal monthlyRate = interestRate / loanTermMonths / 100; // Convert annual interest to monthly decimal
            decimal emi = loanAmount * monthlyRate * (decimal)Math.Pow((double)(1 + monthlyRate), loanTermMonths) /
                          (decimal)(Math.Pow((double)(1 + monthlyRate), loanTermMonths) - 1);

            return Math.Round(emi, 2); 
        }

        public async Task<int> GetLoanApplicationsCountByUserIdAsync(Guid userId)
        {
            return await _context.LoanApplications.CountAsync(l => l.UserId == userId);
        }
    }
}
