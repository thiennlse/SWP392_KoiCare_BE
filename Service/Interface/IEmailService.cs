

namespace Service.Interface
{
    public interface IEmailService
    {
        public  Task SendEmailAsync(string recipientEmail, string subject, string body);
    }
}
