using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Services
{
    public interface INotificationService
    {
        Task SendInvoiceNotificationAsync(Invoice invoice);
        Task SendOverdueReminderAsync(Invoice invoice);
        Task SendPaymentConfirmationAsync(Invoice invoice, Payment payment);
        Task SendSms(string phoneNumber, string message);

        Task SendEmail1(string toEmail,string subject, string message);
        Task SendSms1(string phoneNumber, string message);
    }
}
