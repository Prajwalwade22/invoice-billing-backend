using System.ComponentModel.DataAnnotations;

namespace InvoiceBillingSystem.Models
{
    public class SmsRequest
    {
        [Key]
        public string PhoneNumber { get; set; }
        public string Message { get; set; }

        public DateTime? ScheduledTime { get; set; }
    }
}
