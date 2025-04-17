namespace InvoiceBillingSystem.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }


        public decimal? DiscountApplied { get; set; }


        public string Currency { get; set; } 
        public decimal AmountInSelectedCurrency { get; set; }


        public decimal PaidAmount { get; set; }   
        public decimal RemainingAmount { get; set; } 
        public bool IsFullyPaid { get; set; }


        public decimal OverdueInterest {  get; set; }
        public decimal TotalAmountDue { get; set; }


        public bool IsSigned { get; set; }



        public Guid CompanyId { get; set; }
        public User User { get; set; }
        public Company Company { get; set; } 
    }
}
