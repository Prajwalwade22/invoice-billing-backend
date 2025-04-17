namespace InvoiceBillingSystem.Repositories
{
    public interface IDiscountRepository
    {
        Task<decimal> GetDynamicDiscountAsync(Guid userId, decimal invoiceAmount);
    }
}
