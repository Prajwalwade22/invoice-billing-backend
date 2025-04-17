namespace InvoiceBillingSystem.Services
{
    public interface ISmsJobService
    {
        Task ScheduleSmsAsync(string phoneNumber, string message, DateTime? scheduledTime = null);
        Task SendSmsNow(string phoneNumber, string message);

        Task ScheduleEmailAsync(string toEmail,string subject,string message, DateTime? scheduledTime = null);
        Task SendEmail1(string toEmail,string subject,string message);
    }
}
