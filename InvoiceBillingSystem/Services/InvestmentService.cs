using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InvoiceBillingSystem.Services
{
    public class InvestmentService
    {
        private readonly IInvestmentRepository _investmentRepository;

        public InvestmentService(IInvestmentRepository investmentRepository)
        {
            _investmentRepository = investmentRepository;
        }

        public async Task TransferMoneyAsync(InvestmentTransferDto transferDto)
        {
            var user = await _investmentRepository.GetUserByIdAsync(transferDto.CustomerId);
            if (user != null)
            {
                var transaction = new InvestmentTransaction
                {
                    Id = Guid.NewGuid(),
                    CustomerId = transferDto.CustomerId,
                    InvestmentType = transferDto.InvestmentType,
                    Amount = transferDto.Amount,
                    TransactionDate = DateTime.UtcNow,
                    Status = "Pending"
                };
                
                await _investmentRepository.AddTransactionAsync(transaction);
            }
            else
            {
                throw new Exception("User Id not Found.Please Register User First.");
            }
        }
    }
}
