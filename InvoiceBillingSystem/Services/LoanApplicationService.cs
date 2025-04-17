using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;

namespace InvoiceBillingSystem.Services
{
    public class LoanApplicationService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IInvestmentRepository _investmentRepository;

        public LoanApplicationService(ILoanRepository loanRepository,IInvestmentRepository investmentRepository)
        {
            _loanRepository = loanRepository;
            _investmentRepository = investmentRepository;
        }

        public async Task ApplyForLoanAsync(LoanApplicationDto loanApplicationDto)
        {
            var user = await _investmentRepository.GetUserByIdAsync(loanApplicationDto.UserId);
            if (user != null)
            {
                var loanInformation = new LoanApplication
                {
                    Id = Guid.NewGuid(),
                    UserId = loanApplicationDto.UserId,
                    LoanAmount = loanApplicationDto.LoanAmount,
                    InterestRate = loanApplicationDto.InterestRate,
                    LoanTermMonths = loanApplicationDto.LoanTermMonths,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };
                await _loanRepository.AddLoanApplicationAsync(loanInformation);
            }
            else
            {
                throw new Exception("User Id not Found.Please Register User First.");
            }
        }
    }
}
