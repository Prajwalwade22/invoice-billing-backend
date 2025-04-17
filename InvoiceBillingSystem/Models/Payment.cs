namespace InvoiceBillingSystem.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime PaymentDate { get; set; }

        //public string Status => IsSuccessful ? "Completed" : "Failed";
    }
}
