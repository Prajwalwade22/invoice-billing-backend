using InvoiceBillingSystem.DTO;

namespace InvoiceBillingSystem.Services
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto subscriptionDto);
        Task<bool> CancelSubscriptionAsync(Guid subscriptionId);
        Task<bool> RenewSubscriptionAsync(Guid subscriptionId);
    }
}
