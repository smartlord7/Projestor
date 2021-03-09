using Microsoft.AspNet.Identity;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System;
using SendGrid;

namespace AspNetIdentity.WebApi.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        private async Task configSendGridasync(IdentityMessage message)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("projestorproject@gmail.com", "Projestor");
            var subject = "Projestor Account Confirmation";
            var to = new EmailAddress(message.Destination, "User");
            var plainTextContent = "----------------------";
            var htmlContent = "<strong>" + message.Body + "</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}