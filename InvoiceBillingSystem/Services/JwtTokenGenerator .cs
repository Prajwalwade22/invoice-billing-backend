using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InvoiceBillingSystem.Enum;
using InvoiceBillingSystem.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InvoiceBillingSystem.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly string token = string.Empty;
        private readonly IHttpContextAccessor _contextAccessor;

        public JwtTokenGenerator(IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
        }

        public string GenerateToken(Guid userId, string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            //new Claim(ClaimTypes.Role, role)
            new Claim(ClaimTypes.Role,role.ToString())
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],        
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Guid GetUserId()
        {
            if (!string.IsNullOrEmpty(token))
            {
                var principal = GetPrincipalFromAccessToken(token);
                var userId = principal.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value ?? "";
                if (!string.IsNullOrEmpty(userId))
                    return new Guid(userId);
            }
            return Guid.Empty;
        }
        public string GetUserRole()
        {
            var role = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
            return role ?? throw new UnauthorizedAccessException("Role not found in token.");
        }


        public ClaimsPrincipal? GetPrincipalFromAccessToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JsonWebTokenKeys:AccessToken"])),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
