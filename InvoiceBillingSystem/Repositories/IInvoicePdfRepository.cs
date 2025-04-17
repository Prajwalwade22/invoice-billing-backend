using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public interface IInvoicePdfRepository
    {
        Invoice GetInvoiceById(Guid invoiceId);
    }
}
