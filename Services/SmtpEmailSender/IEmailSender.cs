namespace Libra.Services.SmtpEmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
