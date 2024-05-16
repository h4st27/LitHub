using Libra.Services.SmtpEmailSender;
using Quartz;
namespace Libra.Services.Background
{
    public class EmailSendingJob : IJob
    {
        private readonly IEmailSender _emailSender;
        public EmailSendingJob(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _emailSender.SendEmailAsync("popov.yarik.popov@gmail.com", "Hello", "Hi");
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}
