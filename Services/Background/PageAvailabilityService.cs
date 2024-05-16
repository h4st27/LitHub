using Libra.Services.SmtpEmailSender;

namespace Libra.Services.Background
{
    public class PageAvailabilityService : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlToMonitor = "https://moodle3.chmnu.edu.ua/";
        private readonly string _logFilePath = "page_availability.txt";
        public PageAvailabilityService(IHttpClientFactory httpClientFactory, IEmailSender emailSender)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _httpClient.GetAsync(_urlToMonitor, stoppingToken);
                    var isSuccessStatusCode = response.IsSuccessStatusCode;
                    var logMessage = $"{DateTime.Now} - Website {_urlToMonitor} is {(isSuccessStatusCode ? "available" : "not available")}\n";

                    await File.AppendAllTextAsync(_logFilePath, logMessage);

                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                }
                catch (OperationCanceledException ex)
                {
                    await File.AppendAllTextAsync(_logFilePath, ex.Message);
                }
                catch (Exception ex)
                {
                    await File.AppendAllTextAsync(_logFilePath, ex.Message);
                }
            }
        }
    }
}
