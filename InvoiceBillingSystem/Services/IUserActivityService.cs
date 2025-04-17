namespace InvoiceBillingSystem.Services
{
    public interface IUserActivityService
    {
        Task LogUserActivityAsync(Guid userId);
    }
}
