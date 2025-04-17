using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionDto subscriptionDto)
        {
            var subscription = await _subscriptionService.CreateSubscriptionAsync(subscriptionDto);
            return Ok(subscription);
        }

        [HttpPut("cancel/{subscriptionId}")]
        public async Task<IActionResult> CancelSubscription(Guid subscriptionId)
        {
            var result = await _subscriptionService.CancelSubscriptionAsync(subscriptionId);
            return result ? Ok(new { message = "Subscription cancelled" }) : NotFound(new { message = "Subscription not found" });
        }

        [HttpPut("renew/{subscriptionId}")]
        public async Task<IActionResult> RenewSubscription(Guid subscriptionId)
        {
            var result = await _subscriptionService.RenewSubscriptionAsync(subscriptionId);
            return result ? Ok(new { message = "Subscription renewed" }) : NotFound(new { message = "Subscription not found" });
        }
    }
}
