namespace InvoiceBillingSystem.DTO
{
    public class InvestmentTransferDto
    {
        public Guid CustomerId { get; set; }
        public string InvestmentType { get; set; }
        public decimal Amount { get; set; }
    }
}
