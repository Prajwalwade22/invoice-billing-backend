using InvoiceBillingSystem.Models;

namespace InvoiceBillingSystem.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> CreateSubscriptionAsync(Subscription subscription);
        Task<bool> CancelSubscriptionAsync(Guid subscriptionId);
        Task<bool> RenewSubscriptionAsync(Guid subscriptionId);
    }
}
