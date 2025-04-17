namespace InvoiceBillingSystem.Models
{
    public class InvestmentTransaction
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string InvestmentType { get; set; } 
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }
}
