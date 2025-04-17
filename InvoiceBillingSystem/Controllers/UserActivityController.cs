using InvoiceBillingSystem.Repositories;
using InvoiceBillingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActivityController : ControllerBase
    {
        private readonly IUserActivityService _userActivityService;
        private readonly IUserActivityRepository _userActivityRepository;

        public UserActivityController(IUserActivityService userActivityService,IUserActivityRepository userActivityRepository)
        {
            _userActivityService = userActivityService;
            _userActivityRepository = userActivityRepository;
        }

        [HttpPost("log/{userId}")]
        public async Task<IActionResult> LogUserActivity(Guid userId)
        {
            await _userActivityService.LogUserActivityAsync(userId);
            return Ok(new { Message = "User activity logged successfully." });
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetUserActivity(Guid userId)
        {
            var activities = await _userActivityRepository.GetUserActivitiesAsync(userId);
            return Ok(activities);
        }
    }
}
