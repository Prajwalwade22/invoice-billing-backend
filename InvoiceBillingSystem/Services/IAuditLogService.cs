namespace InvoiceBillingSystem.Services
{
    public interface IAuditLogService
    {
        Task LogActionAsync(string action, string performedBy, string details);
    }
}
