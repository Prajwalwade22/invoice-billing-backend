namespace InvoiceBillingSystem.DTO
{
    public class CreateSubscriptionDto
    {
        public Guid UserId { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
    }
}
