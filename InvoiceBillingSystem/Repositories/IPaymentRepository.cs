using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> ProcessPaymentAsync(Payment payment);
        Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId);
        Task<decimal> GetTotalPaidAmountByInvoiceIdAsync(Guid invoiceId);
    }
}
