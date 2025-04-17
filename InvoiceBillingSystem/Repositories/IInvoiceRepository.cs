using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);
        Task<Invoice?> GetInvoiceByIdAsync(Guid invoiceId);
        Task<IEnumerable<Invoice>> GetInvoicesByUserIdAsync(Guid userId);
        Task<bool> UpdateInvoiceStatusAsync(Guid invoiceId, string status);
        Task<IEnumerable<Invoice>> GetOverdueInvoicesAsync();

        Task<User> GetUserByIdAsync(Guid userId);
        Task<Invoice> GetInvoiceWithUserByIdAsync(Guid invoiceId);

        Task UpdateInvoiceAsync(Invoice invoice);

        decimal CalculateOverdueInterest(Invoice invoice);
        Task ApplyOverdueInterestAsync(Guid invoiceId, decimal interestAmount);
    }
}
