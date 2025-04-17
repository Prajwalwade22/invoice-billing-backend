using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Google.Apis.Drive.v3.Data;

namespace InvoiceBillingSystem.Services
{
    public class CurrencyConverterService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _exchangeRateApiUrl;
        private readonly string _baseCurrency;

        public CurrencyConverterService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _exchangeRateApiUrl = _configuration["CurrencySettings:ExchangeRateApiUrl"];
            _baseCurrency = _configuration["CurrencySettings:BaseCurrency"];
        }

        public async Task<decimal> ConvertCurrencyAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            string apiUrl = _configuration["CurrencySettings:ExchangeRateApiUrl"];

            var response = await _httpClient.GetStringAsync(apiUrl);
            var exchangeData = JObject.Parse(response);

            if (exchangeData["rates"][fromCurrency] == null || exchangeData["rates"][toCurrency] == null)
            {
                throw new Exception($"Currency '{toCurrency}' not supported.");
            }

            decimal fromRate = exchangeData["rates"][fromCurrency].Value<decimal>();
            decimal toRate = exchangeData["rates"][toCurrency].Value<decimal>();

            // Convert currency
            decimal convertedAmount = (amount / fromRate) * toRate;
            return Math.Round(convertedAmount, 2); 
        }


        //private async Task<Dictionary<string, decimal>> GetExchangeRates()
        //{
        //    string apiUrl = "https://api.exchangerate-api.com/v4/latest/INR"; // INR as base
        //    var response = await _httpClient.GetStringAsync(apiUrl);
        //    var rates = JObject.Parse(response)["rates"].ToObject<Dictionary<string, decimal>>();

        //    return rates ?? new Dictionary<string, decimal>();
        //}

    }
}
