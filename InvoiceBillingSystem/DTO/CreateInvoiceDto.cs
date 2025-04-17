namespace InvoiceBillingSystem.DTO
{
    public class CreateInvoiceDto
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }

        public string Currency { get; set; }

        public Guid CompanyId { get; set; }
    }
}
