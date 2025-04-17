namespace InvoiceBillingSystem.DTO
{
    public class LoanApplicationDto
    {
        public Guid UserId { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanTermMonths { get; set; }
        public double InterestRate { get; set; }
        public string LoanType { get; set; } 
    }
}
