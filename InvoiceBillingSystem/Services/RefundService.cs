using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;

namespace InvoiceBillingSystem.Services
{
    public class RefundService : IRefundService
    {
        private readonly ICreditNoteRepository _creditNoteRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPaymentRepository _paymentRepository;

        public RefundService(ICreditNoteRepository creditNoteRepository, IInvoiceRepository invoiceRepository, IPaymentRepository paymentRepository)
        {
            _creditNoteRepository = creditNoteRepository;
            _invoiceRepository = invoiceRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task IssueRefundAsync(Guid invoiceId, decimal refundAmount, string reason)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(invoiceId);
            if (invoice == null)
                throw new Exception("Invoice not found.");

            var totalPaid = await _paymentRepository.GetTotalPaidAmountByInvoiceIdAsync(invoiceId);
            if (refundAmount > totalPaid)
                throw new Exception("Refund amount cannot exceed total paid amount.");

            var creditNote = new CreditNote
            {
                InvoiceId = invoiceId,
                Amount = refundAmount,
                Reason = reason,
                IsRefunded = true
            };

            await _creditNoteRepository.CreateCreditNoteAsync(creditNote);

            invoice.Amount -= refundAmount;
            if (invoice.Amount <= 0)
            {
                invoice.Status = "Refunded";
            }

            await _invoiceRepository.UpdateInvoiceAsync(invoice);
        }
    }

}
