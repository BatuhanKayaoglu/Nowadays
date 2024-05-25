namespace Nowadays.API.Services.EmailSender
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}