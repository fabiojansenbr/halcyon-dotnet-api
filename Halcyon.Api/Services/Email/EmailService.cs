using Halcyon.Api.Extensions;
using Halcyon.Api.Settings;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Halcyon.Api.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IStringLocalizer<EmailService> _localizer;
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;

        public EmailService(
            IStringLocalizer<EmailService> localizer,
            ILogger<EmailService> logger,
            IOptions<EmailSettings> emailSettings)
        {
            _localizer = localizer;
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendAsync(IEmailModel model)
        {
            var subject = _localizer[$"{model.Template}_Subject"].ToString().Replace(model);
            var html = _localizer[$"{model.Template}_Html"].ToString().Replace(model);

            var message = new MailMessage();
            message.To.Add(model.ToAddress);
            message.From = new MailAddress(_emailSettings.NoReplyAddress);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = html;

            try
            {
                using (var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port))
                {
                    client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                    await client.SendMailAsync(message);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Email Send Failed");
            }
        }
    }
}