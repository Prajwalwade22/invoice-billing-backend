using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Services
{
    public interface IDiscountService
    {
        Task<decimal> ApplyDynamicDiscountAsync(Invoice invoice);
    }
}
