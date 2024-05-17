using LitHub.Services.SmtpEmailSender;

namespace LitHub.Services.Background
{
    public class PageAvailabilityService : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlToMonitor = "https://www.youtube.com";
        private readonly string _logFilePath = "checkedpage.txt";
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
                    var logMessage = $"URL {_urlToMonitor} is {(isSuccessStatusCode ? "online" : "offline")}-{DateTime.Now}\n";

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
