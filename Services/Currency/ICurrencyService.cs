using LitHub.Models;

namespace LitHub.Services.Currency
{
    public interface ICurrencyService
    {
        Task<ExchangeRateResponse> GetCurrencyRates();
    }
}
