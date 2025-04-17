using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;

namespace InvoiceBillingSystem.Services
{

    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<SubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto subscriptionDto)
        {
            var subscription = new Subscription
            {
                Id = Guid.NewGuid(),
                UserId = subscriptionDto.UserId,
                PlanName = subscriptionDto.PlanName,
                Price = subscriptionDto.Price,
                ExpiryDate = DateTime.UtcNow.AddMonths(1),
                CreatedAt = DateTime.UtcNow
            };

            await _subscriptionRepository.CreateSubscriptionAsync(subscription);
            return new SubscriptionDto
            {
                Id = subscription.Id,
                UserId = subscription.UserId,
                PlanName = subscription.PlanName,
                Price = subscription.Price,
                ExpiryDate = subscription.ExpiryDate
            };
        }
        public async Task<bool> CancelSubscriptionAsync(Guid subscriptionId)
        {
            return await _subscriptionRepository.CancelSubscriptionAsync(subscriptionId);
        }

        public async Task<bool> RenewSubscriptionAsync(Guid subscriptionId)
        {
            return await _subscriptionRepository.RenewSubscriptionAsync(subscriptionId);
        }
    }
}

