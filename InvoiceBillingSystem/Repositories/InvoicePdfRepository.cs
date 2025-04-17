using InvoiceBillingSystem.Data;
using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public class InvoicePdfRepository : IInvoicePdfRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoicePdfRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Invoice GetInvoiceById(Guid invoiceId)
        {
            return _context.Invoices.FirstOrDefault(i => i.Id == invoiceId);
        }
    }
}
