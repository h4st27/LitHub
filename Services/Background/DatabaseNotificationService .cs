using Libra.Services.DataBaseService;
using Libra.Services.SmtpEmailSender;
using Microsoft.EntityFrameworkCore;

namespace Libra.Services.Background
{
    public class DatabaseNotificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseNotificationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    var latestRecord = dbContext.Model;

                    if (latestRecord != null)
                    {
                        string subject = "Postgres";
                        string body = $"Data in database has been changed.";
                        await emailSender.SendEmailAsync("popov.yarik.popov@gmail.com", subject, body);
                    }

                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }
    }
}
