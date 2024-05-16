using Libra.Models;
using System.Text.Json;

namespace Libra.Services.Currency
{
    public class CurrencyService: ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public CurrencyService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<ExchangeRateResponse> GetCurrencyRates()
        {
            string url = _config["API"];

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                ExchangeRateResponse content = await response.Content.ReadFromJsonAsync<ExchangeRateResponse>();
                return content;
            }
            throw new Exception("Failed to retrieve currency rates.");
        }
    }
}