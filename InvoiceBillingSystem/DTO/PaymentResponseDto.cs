namespace InvoiceBillingSystem.DTO
{
    public class PaymentResponseDto
    {
        public string TransactionId { get; set; }
        public bool IsSuccessful { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
