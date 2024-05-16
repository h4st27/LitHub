using Libra.Services.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Libra.Services.Background
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationBackgroundService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentTime = DateTimeOffset.Now.ToString("HH:mm:ss");

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"New message sent at {currentTime}");

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}
