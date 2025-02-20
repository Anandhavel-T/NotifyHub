using NotifyHub.Infrastructure.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Configuration;

namespace NotifyHub.Infrastructure.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        public EmailService()
        {
            _smtpServer = ConfigurationManager.AppSettings["Email:SmtpServer"];
            _smtpPort = int.Parse(ConfigurationManager.AppSettings["Email:SmtpPort"]);
            _senderEmail = ConfigurationManager.AppSettings["Email:SenderEmail"];
            _senderPassword = ConfigurationManager.AppSettings["Email:SenderPassword"];
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpServer))
            {
                client.Port = _smtpPort;
                client.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                client.EnableSsl = true;

                var message = new MailMessage(_senderEmail, to, subject, body)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(message);
            }
        }
    }
}