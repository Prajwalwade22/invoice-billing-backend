using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public interface ICreditNoteRepository
    {
        Task CreateCreditNoteAsync(CreditNote creditNote);
        Task<List<CreditNote>> GetCreditNotesByInvoiceIdAsync(Guid invoiceId);
    }
}
