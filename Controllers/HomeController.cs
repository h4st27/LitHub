using Libra.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Libra.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubContext<CurrencyHub> _hubContext;

        public HomeController(IHubContext<CurrencyHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task<IActionResult> Index()
        {
            await _hubContext.Clients.All.SendAsync("GetCurrencyData");
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
