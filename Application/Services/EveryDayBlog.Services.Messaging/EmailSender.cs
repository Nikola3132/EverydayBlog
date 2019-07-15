namespace EveryDayBlog.Services.Messaging
{
    using System.Threading.Tasks;

    using EveryDayBlog.Services.Messaging.Settings;
    using global::SendGrid;
    using global::SendGrid.Helpers.Mail;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;

    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            this.Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return this.Execute(this.Options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("everydayblog.website@gmail.com", "EveryDayBlog"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message,
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
