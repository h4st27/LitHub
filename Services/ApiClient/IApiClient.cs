namespace MyApp.Services.ApiClient
{
    public interface IApiClient
    {
        Task<T?> GetAsync<T>(string url);
    }

}
