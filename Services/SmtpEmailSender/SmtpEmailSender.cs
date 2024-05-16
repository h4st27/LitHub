using System.Net.Mail;
using System.Net;

namespace Libra.Services.SmtpEmailSender
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "clemsonnn22@gmail.com";
        private readonly string _smtpPassword = "secret";

        public SmtpEmailSender()
        {
            _smtpClient = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUsername),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
