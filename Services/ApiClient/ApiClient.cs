namespace Libra.Services.ApiClient
{
    public class ApiClient: IApiClient
    {
        private readonly HttpClient _httpClient;
        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<T?> GetAsync<T>(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
