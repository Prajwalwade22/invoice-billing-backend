namespace InvoiceBillingSystem.DTO
{
    public class ReportSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public int PendingInvoices { get; set; }
        public decimal TotalTaxCollected { get; set; }
        public List<TopCustomerDto> TopCustomers { get; set; }
    }
}
