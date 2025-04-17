using InvoiceBillingSystem.DTO;

namespace InvoiceBillingSystem.Repositories
{
    public interface IReportRepository
    {
        Task<decimal> GetTotalRevenueAsync(DateTime? startDate, DateTime? endDate);
        Task<int> GetPendingInvoiceCountAsync(DateTime? startDate, DateTime? endDate);
        Task<decimal> GetTotalTaxCollectedAsync(DateTime? startDate, DateTime? endDate);
        Task<List<TopCustomerDto>> GetTopCustomersAsync(DateTime? startDate, DateTime? endDate);
    }
}
