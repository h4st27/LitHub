using Microsoft.Extensions.Caching.Memory;

namespace Libra.Services.Background
{
    public class ExternalApiService : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;

        public ExternalApiService(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_cache.TryGetValue<string>("apiResponse", out var cachedValue))
                {
                    Console.WriteLine($"Cached API Response: {cachedValue}");
                }
                else
                {
                    Console.WriteLine("No cached value found.");
                }
                var apiResponse = await CallExternalApi();

                _cache.Set("apiResponse", apiResponse, TimeSpan.FromMinutes(10));
                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }

        private async Task<string> CallExternalApi()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync("https://api.currencyapi.com/v3/latest?apikey=cur_live_nQQGjFl9qCjB0gWBlTbuZpXXkU1P1hMOL9OR7BOv&currencies=EUR%2CUSD%2CCAD");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }

            return null;
        }
    }
}
