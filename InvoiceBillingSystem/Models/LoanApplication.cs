namespace InvoiceBillingSystem.Models
{
    public class LoanApplication
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanTermMonths { get; set; }
        public double InterestRate { get; set; }
        public string LoanType { get; set; }
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public decimal EMI { get; set; }
    }
}
