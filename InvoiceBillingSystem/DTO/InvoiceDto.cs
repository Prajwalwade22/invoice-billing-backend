namespace InvoiceBillingSystem.DTO
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Currency { get; set; }
        public decimal AmountInSelectedCurrency {  get; set; }
        public Guid CompanyId { get; set; }
    }
}
