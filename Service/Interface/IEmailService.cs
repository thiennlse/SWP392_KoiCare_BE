

namespace Service.Interface
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string recipientEmail, string body);
        Task SendVerifyAccountEmail(string email, string orderCode);
    }
}
