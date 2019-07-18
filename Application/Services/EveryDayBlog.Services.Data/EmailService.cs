namespace EveryDayBlog.Services.Data
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using MimeKit;

    public class EmailService : IEmailService
    {
        private readonly IHostingEnvironment env;
        private readonly IEmailSender sendGridEmailSender;

        public EmailService(IHostingEnvironment env, IEmailSender sendGridEmailSender)
        {
            this.env = env;
            this.sendGridEmailSender = sendGridEmailSender;
        }

        public async Task<bool> SendEmailToUser(string callBackUrl, string email)
        {
            var builder = new BodyBuilder();

            var pathToFile = this.env.WebRootPath
                        + Path.DirectorySeparatorChar.ToString()
                        + "Templates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "EmailTemplate"
                        + Path.DirectorySeparatorChar.ToString()
                        + "Confirm_EmailTemplate.html";


            using (StreamReader sourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = await sourceReader.ReadToEndAsync();
            }

            string messageBody = string.Format(
                builder.HtmlBody,
                callBackUrl);

            await this.sendGridEmailSender.SendEmailAsync(
                email,
                "Confirm your email",
                messageBody);

            return true;
        }
    }
}
