namespace InvoiceBillingSystem.Services
{
    public interface IRefundService
    {
        Task IssueRefundAsync(Guid invoiceId, decimal refundAmount, string reason);
    }
}
