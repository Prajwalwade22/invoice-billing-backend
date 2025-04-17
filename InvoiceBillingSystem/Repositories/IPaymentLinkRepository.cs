using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public interface IPaymentLinkRepository
    {
        Task<PaymentLink?> GetByInvoiceIdAsync(Guid invoiceId);
        Task<PaymentLink?> GetByIdAsync(Guid paymentLinkId);
        Task SavePaymentLinkAsync(PaymentLink paymentLink);
        Task UpdatePaymentLinkAsync(PaymentLink paymentLink);
        Task<PaymentLink> GetActivePaymentLinkByInvoiceIdAsync(Guid invoiceId);
    }
}
