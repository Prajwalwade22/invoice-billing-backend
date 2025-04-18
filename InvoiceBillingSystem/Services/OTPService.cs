using InvoiceBillingSystem.Models;
using System.Security.Cryptography;
using InvoiceBillingSystem.Repositories;
using System.Net.Mail;
using System.Net;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Google.Apis.Drive.v3.Data;

namespace InvoiceBillingSystem.Services
{
    public class OTPService:IOtpService
    {
        private readonly IOtpRepository _Otprepository;

        private readonly INotificationService _notificationService;

        private readonly IConfiguration _configuration;

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public Guid UserId;


        public OTPService(IOtpRepository otpRepository, INotificationService notificationService, IConfiguration configuration, IJwtTokenGenerator jwtTokenGenerator)
        {
            _Otprepository = otpRepository;
            _notificationService = notificationService;
            _configuration = configuration;
        }

        private string GenerateOTP()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                int otp = BitConverter.ToUInt16(bytes, 0) % 1000000;
                return otp.ToString("D6"); 
            }
        }

        public async Task<bool> GenerateAndSendOTPAsync(Guid userid,string email, string phone)
        {
            string otpCode = GenerateOTP();
            var otp = new OTP
            {
                UserId = userid,
                Code = otpCode,
                CreatedAt=DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMinutes(20)
            };

            await _Otprepository.MarkExpiredUnverifiedOtpsAsVerifiedAsync();
            await _Otprepository.SaveOTPAsync(otp);

            string message = $"Enter Your OTP Code At The Time Of Login.The Code Is: {otpCode}. It is valid for 5 minutes.";

            await SendEmail(email, "Your OTP Code", message);
            await SendSms(phone, message);

            return true;
        }

        public async Task<bool> VerifyOTPAsync(Guid userId, string otpCode)
        {
            var otp = await _Otprepository.GetOTPAsync(userId, otpCode);
            if (otp == null) return false; 

            await _Otprepository.MarkOTPAsVerifiedAsync(otp);
            return true;
        }

        private async Task SendEmail(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient
            {
                Host = _configuration["Email:SmtpHost"],
                Port = int.Parse(_configuration["Email:SmtpPort"]),
                Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Email:SenderEmail"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

        private async Task SendSms(string phoneNumber, string message)
        {
            TwilioClient.Init(_configuration["Twilio:AccountSid"], _configuration["Twilio:AuthToken"]);

            await MessageResource.CreateAsync(
                body: message,
                from: new Twilio.Types.PhoneNumber(_configuration["Twilio:FromNumber"]),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );
        }
    }

}
