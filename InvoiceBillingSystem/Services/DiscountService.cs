using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;

namespace InvoiceBillingSystem.Services
{
    public class DiscountService:IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<decimal> ApplyDynamicDiscountAsync(Invoice invoice)
        {
            decimal discountAmount = await _discountRepository.GetDynamicDiscountAsync(invoice.UserId, invoice.Amount);
            invoice.DiscountApplied = discountAmount;
            return discountAmount;
        }
    }
}
