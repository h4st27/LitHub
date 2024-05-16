using Libra.Models;

namespace Libra.Services.Currency
{
    public interface ICurrencyService
    {
        Task<ExchangeRateResponse> GetCurrencyRates();
    }
}
