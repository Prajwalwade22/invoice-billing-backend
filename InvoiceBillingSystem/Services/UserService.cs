using System.Numerics;
using Google.Apis.Drive.v3.Data;
using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Enum;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;
using Microsoft.AspNet.Identity;

namespace InvoiceBillingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ICompanyRepository _companyRepository;
        private readonly IOtpService _OtpService;
        private readonly IUserActivityService _userActivityService;
        private readonly IAuditLogService _AuditlogService;
        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator,ICompanyRepository companyRepository,IOtpService otpService,IUserActivityService userActivityService,IAuditLogService auditLogService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _companyRepository = companyRepository;
            _OtpService = otpService;
            _userActivityService = userActivityService;
            _AuditlogService = auditLogService;
        }

        public async Task<string> RegisterUserAsync(UserDto userDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            var company = await _companyRepository.GetCompanyByIdAsync(userDto.CompanyId);
            if (company == null)
            {
                throw new Exception("Invalid Company ID.");
            }

            var user = new Models.User
            {
                Id = Guid.NewGuid(),
                FullName = userDto.FullName,
                Email = userDto.Email,
                PasswordHash = _passwordHasher.HashPassword(userDto.Password),
                //Role = "Customer",
                Role = UserRoles.Customer.ToString(),
                CompanyId = userDto.CompanyId,
                Phone= userDto.Phone,
            };

            await _userActivityService.LogUserActivityAsync(user.Id);
            await _AuditlogService.LogActionAsync("Registeration Done", user.Email, "Registeration Done And OTP Is Send To Email And MobileNumber");
            await _userRepository.CreateUserAsync(user);
            await _OtpService.GenerateAndSendOTPAsync(user.Id,user.Email, user.Phone);
            //await _userRepository.CreateUserAsync(user);
            return "User registered successfully.";
        }

    


        public async Task<string> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password.");
            }

            if (string.IsNullOrWhiteSpace(loginDto.Code))
            {
                await _OtpService.GenerateAndSendOTPAsync(user.Id, user.Email, user.Phone);
                throw new Exception("OTP sent. Please enter OTP to continue.");
            }

            bool isOtpVerified = await _OtpService.VerifyOTPAsync(user.Id, loginDto.Code);
            if (!isOtpVerified)
            {
                throw new Exception("OTP verification failed. Please enter a valid OTP.");
            }

            await _AuditlogService.LogActionAsync("User logged in successfully", user.Email, "via email authentication");
            await _userActivityService.LogUserActivityAsync(user.Id);

            return _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.Role);
        }


        public async Task<bool> AssignRoleAsync(Guid userId, string role)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            user.Role = role;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<string> RegisterAdminAsync(AdminRegisterDto adminRegisterDto)
        {
           var admin=await _userRepository.GetAdminByEmailAsync(adminRegisterDto.Email);
            if (admin != null)
            {
                throw new Exception("User already exists.");
            }

            var company = await _companyRepository.GetCompanyByIdAsync(adminRegisterDto.Company_Id);
            if (company == null)
            {
                throw new Exception("Invalid Company ID.");
            }

            var admin1 = new Admin
            {
                AdminId = Guid.NewGuid(),
                Name = adminRegisterDto.Name,
                Email = adminRegisterDto.Email,
                Password = _passwordHasher.HashPassword(adminRegisterDto.Password),
                Role = UserRoles.Admin.ToString(),
                Phone = adminRegisterDto.Phone,
                Gender = adminRegisterDto.Gender,
                Company_Id = adminRegisterDto.Company_Id,
                CreatedAt = DateTime.UtcNow
            };

            await _userActivityService.LogUserActivityAsync(admin1.AdminId);
            await _AuditlogService.LogActionAsync("Registeration Done", admin1.Email, "Registeration Done");
            await _userRepository.CreateAdminAsync(admin1);
            return "Admin registered successfully.";
        }

        public async Task<string> LoginAdminAsync(AdminLoginDto adminLoginDto)
        {
            var admin=await _userRepository.GetAdminByEmailAsync(adminLoginDto.Email);

            if (admin == null || !_passwordHasher.VerifyPassword(adminLoginDto.Password,admin.Password))
            {
                throw new Exception("Invalid email or password.");
            }
            await _AuditlogService.LogActionAsync("User logged in successfully", admin.Email, "via email authentication");
            await _userActivityService.LogUserActivityAsync(admin.AdminId);

            return _jwtTokenGenerator.GenerateToken(admin.AdminId, admin.Email, admin.Role);
        }
    }
}
