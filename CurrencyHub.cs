using LitHub.Models;
using LitHub.Services.Currency;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
namespace LitHub
{
    public class CurrencyHub : Hub
    {
        private readonly ICurrencyService _currencyService;
        public CurrencyHub(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task GetCurrencyData()
        {
            var rates = await _currencyService.GetCurrencyRates();
            await Clients.All.SendAsync("ReceiveCurrencyData", rates);
        }
    }
}
