using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Dalleni.Infrastructure.ExternalServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<bool> SendPasswordResetEmailAsync(string? email, string subject, string otp)
        {
            var message = $"Use this OTP to reset your password: {otp}";
            return SendEmailAsync(email, subject, message);
        }

        public async Task<bool> SendEmailAsync(string? email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, true);
                await client.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = message,
                    TextBody = message,
                };

                var mimeMessage = new MimeMessage
                {
                    Body = bodyBuilder.ToMessageBody(),
                    Subject = subject
                };

                mimeMessage.From.Add(new MailboxAddress("Dalleni Team", _emailSettings.FromEmail));
                mimeMessage.To.Add(MailboxAddress.Parse(email));

                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", email);
                return false;
            }
        }
    }
}
