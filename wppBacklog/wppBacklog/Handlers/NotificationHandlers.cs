using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;
using wppBacklog.Handlers.Interfaces;
using wppBacklog.Models;

namespace wppBacklog.Handlers
{
    public class NotificationHandlers : INotificationHandlers
    {
        private readonly IOptions<AppConfigModel> _config;

        public NotificationHandlers(IOptions<AppConfigModel> config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            SendGridClient client = new SendGridClient(_config.Value.SendGrid.ApiKey);
            EmailAddress from = new EmailAddress("support@7thdata.com", "BACKLOG by SEVENTH");
            EmailAddress to = new EmailAddress(email, email);
            string plainTextContent = message;
            string htmlContent = message;
            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            Response response = await client.SendEmailAsync(msg);

            Debug.WriteLine(response.StatusCode.ToString());
        }

    }
}
