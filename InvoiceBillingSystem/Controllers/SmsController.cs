using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Twilio.Http;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly ISmsJobService _smsJobService;

        public SmsController(ISmsJobService smsJobService)
        {
            _smsJobService = smsJobService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendSms([FromBody] SmsRequest request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.Message))
            {
                return BadRequest("Phone number and message are required.");
            }

            DateTime? scheduledTime = request.ScheduledTime.HasValue && request.ScheduledTime > DateTime.UtcNow
                                      ? request.ScheduledTime.Value
                                      : (DateTime?)null;

            await _smsJobService.ScheduleSmsAsync(request.PhoneNumber, request.Message, scheduledTime);
            return Ok("SMS scheduled successfully!");
        }

        [HttpPost("SendEmail")]

        public async Task<IActionResult> SendEmail([FromBody] ScheduleEmail scheduleEmail)
        {
            if (string.IsNullOrEmpty(scheduleEmail.toEmail) || string.IsNullOrEmpty(scheduleEmail.Message))
            {
                return BadRequest("Email And Message Are Required");
            }
            DateTime? scheduledTime = scheduleEmail.ScheduledTime.HasValue && scheduleEmail.ScheduledTime > DateTime.UtcNow
            ? scheduleEmail.ScheduledTime.Value
                                     : (DateTime?)null;
            await _smsJobService.ScheduleEmailAsync(scheduleEmail.toEmail,scheduleEmail.Subject, scheduleEmail.Message, scheduledTime);
            return Ok(new { Message = " Email scheduled successfully.", Email =scheduleEmail.toEmail, ScheduledTime = scheduleEmail.ScheduledTime });
        }


    }
}

