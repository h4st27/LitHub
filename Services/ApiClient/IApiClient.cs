namespace LitHub.Services.ApiClient
{
    public interface IApiClient
    {
        Task<T?> GetAsync<T>(string url);
    }

}
