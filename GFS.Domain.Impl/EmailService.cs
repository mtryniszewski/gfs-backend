using System;
using System.Threading.Tasks;
using GFS.Core;
using GFS.Domain.Core;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace GFS.Domain.Impl
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _config;
        public EmailService(IOptions<EmailConfig> emailConfig)
        {
            _config = emailConfig.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_config.FromName, _config.FromAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) {Text = message};

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(_config.MailServerAddress, Convert.ToInt32(_config.MailServerPort));
                
                await client.AuthenticateAsync(_config.UserId, _config.UserPassword);

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}