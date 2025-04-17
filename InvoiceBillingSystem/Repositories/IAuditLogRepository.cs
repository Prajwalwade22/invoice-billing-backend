using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public interface IAuditLogRepository
    {
        Task LogActionAsync(AuditLog log);
        Task<IEnumerable<AuditLog>> GetLogsByUserAsync(string email);
        Task<IEnumerable<AuditLog>> GetLogsByInvoiceAsync(Guid id);
    }
}
