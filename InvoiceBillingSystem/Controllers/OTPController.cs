using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        private readonly IOtpService _otpService;

        public OTPController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateOTP([FromBody] OTPRequestDto otpRequest)
        {
            bool success = await _otpService.GenerateAndSendOTPAsync(otpRequest.UserId, otpRequest.Email, otpRequest.Phone);
            if (!success) return BadRequest("Failed to generate OTP.");

            return Ok(new { Message = "OTP Sent Successfully!" });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyOTP([FromBody] OTPVerifyDto otpVerify)
        {
            bool isValid = await _otpService.VerifyOTPAsync(otpVerify.UserId, otpVerify.OTP);
            if (!isValid) return BadRequest("Invalid or Expired OTP.");

            return Ok(new { Message = "OTP Verified Successfully!" });
        }
    }
}

