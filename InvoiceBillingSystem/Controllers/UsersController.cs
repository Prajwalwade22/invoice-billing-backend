using InvoiceBillingSystem.Attributes;
using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Enum;
using InvoiceBillingSystem.Repositories;
using InvoiceBillingSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IForgotPasswordService _forgotPasswordService;
        private readonly INotificationService _notificationService;
        private readonly IUserRepository _userRepository;

        public UsersController(IUserService userService,IForgotPasswordService forgotPasswordService,INotificationService notificationService,IUserRepository userRepository)
        {
            _userService = userService;
            _forgotPasswordService = forgotPasswordService;
            _notificationService = notificationService;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var result = await _userService.RegisterUserAsync(userDto);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _userService.LoginUserAsync(loginDto);
            return Ok(new { token });
        }

        [HttpPost("AssignRole")]
        //[RequireRole("Admin")]
        //[Authorize(Roles = nameof(UserRoles.Admin))]
        public async Task<IActionResult> AssignRole(Guid userId, string role)
        {
            var success = await _userService.AssignRoleAsync(userId, role);
            if (!success) return BadRequest("We have been very Failed to assign role.");

            return Ok("The Role has been assigned successfully.");
        }


        [HttpPost("AdminRegister")]
        [AllowAnonymous]
        public async Task<IActionResult> AdminRegister([FromBody] AdminRegisterDto adminRegisterDto)
        {
            var result = await _userService.RegisterAdminAsync(adminRegisterDto);
            return Ok(new { message = result });
        }

        [HttpPost("AdminLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto adminLoginDto)
        {
            var token = await _userService.LoginAdminAsync(adminLoginDto);
            return Ok(new { token });
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            var token = await _forgotPasswordService.GenerateResetTokenAsync(dto.Email);
            if (token == null)
                return NotFound("User not found.");
            var user=await _userRepository.GetUserByEmailAsync(dto.Email);

            //await _notificationService.SendEmail1(dto.Email, dto.Email, $"Your reset token is: {token}");

            var link = $"http://localhost:4200/login?email={dto.Email}&token={token}";
            await _notificationService.SendEmail1(dto.Email, "Password Reset Link", $"Click to reset your password: {link}");


            await _notificationService.SendSms1(user.Phone, $"Your reset token is: {token}");

            return Ok(new { message = "Reset token generated.", token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var success = await _forgotPasswordService.ResetPasswordAsync(dto);
            if (!success)
                return BadRequest("Invalid or expired token.");

            return Ok("Password reset successful.");
        }
    }
}
