using System.Net;
using System.Net.Mail;

namespace Nowadays.API.Services.EmailSender
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            string? mail = _configuration["Email:From"];
            var pw = _configuration["Email:Password"];

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(mail, pw),
                EnableSsl = true
            };

            return client.SendMailAsync(
                               new MailMessage(mail!, email)
                               {
                                   Subject = subject,
                                   Body = message
                               });
        }
    }
}
