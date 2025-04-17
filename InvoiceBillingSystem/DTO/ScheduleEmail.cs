namespace InvoiceBillingSystem.DTO
{
    public class ScheduleEmail
    {
        public string toEmail { get; set; }
        public string Message { get; set; }

        public string Subject { get; set; }
        public DateTime? ScheduledTime { get; set; }
    }
}
