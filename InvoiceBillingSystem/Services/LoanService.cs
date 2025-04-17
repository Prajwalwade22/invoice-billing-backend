using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;

namespace InvoiceBillingSystem.Services
{
    public class LoanService
    {

        private readonly ILoanRepository _loanRepository;
        private readonly IInvestmentRepository _investmentRepository;

        public LoanService(ILoanRepository loanRepository,IInvestmentRepository investmentRepository)
        {
            _loanRepository = loanRepository;
            _investmentRepository = investmentRepository;
        }

        public decimal CalculateEMI(decimal loanAmount, decimal interestRate, int months)
        {
            decimal monthlyRate = interestRate / 12 / 100;
            return (loanAmount * monthlyRate * (decimal)Math.Pow((double)(1 + monthlyRate), months)) /
                   ((decimal)Math.Pow((double)(1 + monthlyRate), months) - 1);
        }
        public async Task<LoanApplication> ApplyForLoanAsync(LoanApplicationDto loanDto)
        {
            var user = await _investmentRepository.GetUserByIdAsync(loanDto.UserId);
            var existingLoanApplicationsCount = await _loanRepository.GetLoanApplicationsCountByUserIdAsync(loanDto.UserId);
            if (user != null && existingLoanApplicationsCount < 3)
            {
                var loanApplication = new LoanApplication
                {
                    Id = Guid.NewGuid(),
                    UserId = loanDto.UserId,
                    LoanAmount = loanDto.LoanAmount,
                    LoanTermMonths = loanDto.LoanTermMonths,
                    InterestRate = loanDto.InterestRate,
                    LoanType = loanDto.LoanType,
                    Status = "Pending",
                };

                return await _loanRepository.ApplyForLoanAsync(loanApplication);
            }
            else
            {
                throw new InvalidOperationException("USER DOES NOT EXIST ? PLEASE REGISTER USER FIRST\nOR\nYOU HAVE EXIST THE LIMIT OF LOAN APPLICATIONS");
            }
        }
        

        public async Task<IEnumerable<LoanApplication>> GetLoanApplicationsByUserIdAsync(Guid userId)
        {
            return await _loanRepository.GetLoanApplicationsByUserIdAsync(userId);
        }

        public async Task<bool> UpdateLoanStatusAsync(Guid loanId, string status)
        {
            return await _loanRepository.UpdateLoanStatusAsync(loanId, status);
        }
    }
}
