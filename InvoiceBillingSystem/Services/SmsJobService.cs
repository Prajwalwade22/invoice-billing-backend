using Hangfire;
using iText.Layout.Properties;

namespace InvoiceBillingSystem.Services
{
    public class SmsJobService:ISmsJobService
    {
        private readonly INotificationService _notificationService;


        public SmsJobService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task ScheduleSmsAsync(string phoneNumber, string message, DateTime? scheduledTime = null)
        {
            if (scheduledTime.HasValue && scheduledTime.Value > DateTime.UtcNow)
            {
                BackgroundJob.Schedule(() => SendSmsNow(phoneNumber, message), scheduledTime.Value);
            }
            else
            {
                BackgroundJob.Enqueue(() => SendSmsNow(phoneNumber, message));
            }
        }

        public async Task SendSmsNow(string phoneNumber, string message)
        {
            await _notificationService.SendSms(phoneNumber, message);
        }


        public async Task ScheduleEmailAsync(string toEmail, string subject, string message, DateTime? scheduledTime = null)
        {
            if (scheduledTime.HasValue && scheduledTime.Value > DateTime.UtcNow)
            {
                BackgroundJob.Schedule(() => SendEmail1(toEmail,subject,message), scheduledTime.Value);
            }
            else
            {
                BackgroundJob.Enqueue(() => SendEmail1(toEmail,subject,message));
            }
        }


        public async Task SendEmail1(string toEmail,string subject,string message)
        {
            await _notificationService.SendEmail1(toEmail,subject,message);
        }
    }
}
