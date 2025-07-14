using Microsoft.Extensions.Options;
using MimeKit;
using PharmacyManagementSystem.Configurations;
using PharmacyManagementSystem.Interfaces;
using MailKit.Net.Smtp;

namespace PharmacyManagementSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = body };
                email.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }

    }
}
