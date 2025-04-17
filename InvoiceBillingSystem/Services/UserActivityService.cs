using System.Net;
using System.Text.RegularExpressions;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;
using Newtonsoft.Json.Linq;
using UAParser;

namespace InvoiceBillingSystem.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public UserActivityService(IUserActivityRepository userActivityRepository, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _userActivityRepository = userActivityRepository;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public async Task LogUserActivityAsync(Guid userId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return;

            string userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            string ipAddress = await GetClientIpAddress(httpContext);
            string networkAddress = GetLocalNetworkAddress();
            var geoData = await GetGeolocationAsync(ipAddress);

            var lastActivity = await _userActivityRepository.GetLastActivityByUserAsync(userId);

            if (lastActivity != null && lastActivity.ActivityEndTime == null)
            {
                lastActivity.ActivityEndTime = DateTime.UtcNow;
                lastActivity.SessionDuration = (lastActivity.ActivityEndTime.Value - lastActivity.ActivityTime).TotalMinutes;

                await _userActivityRepository.UpdateActivityAsync(lastActivity);
            }

            var deviceInfo = GetDeviceInfo(userAgent);

            var activity = new UserActivity
            {
                UserId = userId,
                IpAddress = ipAddress,
                NetworkAddress = networkAddress,
                Device = deviceInfo.Device,
                Browser = deviceInfo.Browser,
                OS = deviceInfo.OS,
                Country = geoData?.Country ?? "Unknown",
                City = geoData?.City ?? "Unknown",
                ISP = geoData?.ISP ?? "Unknown",
                ASN = geoData?.ASN ?? "Unknown",
                Organization = geoData?.Organization ?? "Unknown",
                Region = geoData?.Region ?? "Unknown",

                ActivityTime = DateTime.UtcNow,
                ActivityEndTime = null,
                SessionDuration = null
            };

            await _userActivityRepository.LogActivityAsync(activity);
        }

        private async Task<string> GetClientIpAddress(HttpContext httpContext)
        {
            try
            {
                if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    var forwardedIp = httpContext.Request.Headers["X-Forwarded-For"].ToString().Split(',')[0].Trim();
                    if (!IsPrivateIP(forwardedIp)) // Ensure it's not a private IP
                        return forwardedIp;
                }

                var remoteIp = httpContext.Connection.RemoteIpAddress?.ToString();
                if (!string.IsNullOrEmpty(remoteIp) && !IsPrivateIP(remoteIp))
                    return remoteIp;

                string publicIp = await _httpClient.GetStringAsync("https://api64.ipify.org");
                return publicIp.Trim();
            }
            catch
            {
                return "Unknown";
            }
        }


        private string GetLocalNetworkAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "Unknown";
        }

        private async Task<GeoLocationResponse> GetGeolocationAsync(string ipAddress)
        {
            if (IsPrivateIP(ipAddress))
                return new GeoLocationResponse
                {
                    Country = "Local Network",
                    City = "Local",
                    Region = "Local",
                    ISP = "Private IP",
                    ASN = "N/A",
                    Organization = "N/A"
                };

            try
            {
                string apiUrl = $"http://ip-api.com/json/{ipAddress}?fields=status,country,regionName,city,isp,as,org";
                var response = await _httpClient.GetStringAsync(apiUrl);
                var json = JObject.Parse(response);

                if (json["status"]?.ToString() != "success")
                    return new GeoLocationResponse
                    {
                        Country = "Unknown",
                        City = "Unknown",
                        Region = "Unknown",
                        ISP = "Unknown",
                        ASN = "Unknown",
                        Organization = "Unknown"
                    };

                return new GeoLocationResponse
                {
                    Country = json["country"]?.ToString() ?? "N/A",
                    City = json["city"]?.ToString() ?? "N/A",
                    Region = json["regionName"]?.ToString() ?? "N/A",
                    ISP = json["isp"]?.ToString() ?? "N/A",
                    ASN = json["as"]?.ToString() ?? "N/A",
                    Organization = json["org"]?.ToString() ?? "N/A"
                };
            }
            catch (Exception)
            {
                return new GeoLocationResponse
                {
                    Country = "Unknown",
                    City = "Unknown",
                    Region = "Unknown",
                    ISP = "Unknown",
                    ASN = "Unknown",
                    Organization = "Unknown"
                };
            }
        }




        private bool IsPrivateIP(string ipAddress)
        {
            return ipAddress.StartsWith("192.") || ipAddress.StartsWith("10.") ||
                   ipAddress.StartsWith("172.") || ipAddress.StartsWith("127.") || ipAddress == "::1";
        }

        private bool IsValidPublicIP(string ip)
        {
            return !string.IsNullOrEmpty(ip) && !IsPrivateIP(ip) && Regex.IsMatch(ip, @"\b(?:\d{1,3}\.){3}\d{1,3}\b");
        }

        private DeviceInfo GetDeviceInfo(string userAgent)
        {
            var parser = UAParser.Parser.GetDefault();
            ClientInfo clientInfo = parser.Parse(userAgent);

            return new DeviceInfo
            {
                Device = string.IsNullOrEmpty(clientInfo.Device.Family) ? "Unknown" : clientInfo.Device.Family,
                Browser = string.IsNullOrEmpty(clientInfo.UA.Family) ? "Unknown" : clientInfo.UA.Family,
                OS = string.IsNullOrEmpty(clientInfo.OS.Family) ? "Unknown" : clientInfo.OS.Family
            };
        }
    }

    public class GeoLocationResponse
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }  
        public string ISP { get; set; }
        public string ASN { get; set; }  
        public string Organization { get; set; } 
    }


    public class DeviceInfo
    {
        public string Device { get; set; }
        public string Browser { get; set; }
        public string OS { get; set; }
    }
}
