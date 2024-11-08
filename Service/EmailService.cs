using Microsoft.Extensions.Configuration;
using BusinessObject.Models;
using Service.Interface;
using System.Net;
using System.Net.Mail;
using Repository.Interface;

namespace Service
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _senderEmail;
        private readonly string _password;
        private readonly IOrderRepository _orderRepository;

        public EmailService(IConfiguration configuration, IPaymentService paymentService, IOrderRepository orderRepository)
        {
            // Reading SMTP settings from appsettings.json
            _smtpServer = configuration["SMTP:smtpServer"];
            _port = int.Parse(configuration["SMTP:port"]);
            _senderEmail = configuration["SMTP:senderEmail"];
            _password = configuration["SMTP:password"];
            _orderRepository = orderRepository;
        }

        public async Task SendEmailAsync(string recipientEmail, string body)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var client = new SmtpClient(_smtpServer, _port)
            {
                Port = _port,
                Credentials = new NetworkCredential(_senderEmail, _password),
                EnableSsl = true
            })
            using (var mail = new MailMessage(_senderEmail, recipientEmail)
            {
                Subject = "Hóa đơn mua hàng",
                Body = body,
                IsBodyHtml = true
            })
            {
                try
                {
                    await client.SendMailAsync(mail);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    // Log more detailed information if necessary
                }
            }
        }

        public async Task SendVerifyAccountEmail(string email, string orderCode)
        {
            Order order = await _orderRepository.GetOrderByCode(orderCode);
            if (order != null)
            {
                var subject = "Hóa đơn mua hàng";
                string body = $@"
<html>
  <body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px;"">
    <div style=""max-width: 600px; margin: 0 auto; padding: 20px; background-color: #ffffff; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);"">
      <!-- Centered, Circular Logo with Company Name Below -->
      <div style=""text-align: center; margin-bottom: 20px;"">
        <img src=""https://res.cloudinary.com/dkedkbs8d/image/upload/v1729832967/e6hnnizym9xwmr99b96q.png"" alt=""Koicare Logo"" style=""width: 100px; height: 100px; border-radius: 50%; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);"">
      </div>

      <p style=""font-size: 16px;"">Xin chào,</p>
      <p style=""font-size: 16px;"">Đây là hóa đơn mua hàng của bạn:</p>
      <table style=""width: 100%; border-collapse: collapse; margin-top: 20px;"">
        <tr style=""background-color: #f8f8f8;"">
          <td style=""padding: 12px; border: 1px solid #ddd; font-weight: bold;"">Mã đơn hàng:</td>
          <td style=""padding: 12px; border: 1px solid #ddd;"">{order.Code}</td>
        </tr>
        <tr>
          <td style=""padding: 12px; border: 1px solid #ddd; font-weight: bold;"">Ngày thanh toán:</td>
          <td style=""padding: 12px; border: 1px solid #ddd;"">{order.OrderDate:yyyy-MM-dd HH:mm:ss}</td>
        </tr>
        <tr style=""background-color: #f8f8f8;"">
          <td style=""padding: 12px; border: 1px solid #ddd; font-weight: bold;"">Tổng tiền:</td>
          <td style=""padding: 12px; border: 1px solid #ddd;"">{order.TotalCost:#,##0} ₫</td>
        </tr>
        <tr>
          <td style=""padding: 12px; border: 1px solid #ddd; font-weight: bold;"">Trạng thái thanh toán:</td>
          <td style=""padding: 12px; border: 1px solid #ddd;"">{order.Status}</td>
        </tr>
      </table>
      <p style=""font-size: 16px; margin-top: 20px;"">Cám ơn đã sử dụng dịch vụ của chúng tôi.</p>
    </div>
  </body>
</html>"
    ;
                await SendEmailAsync(email, subject, body);
            }
            else
            {
                throw new Exception("OrderCode not found");
            }
        }

        private async Task SendEmailAsync(string email, string subject, string body)
        {
            var senderEmail = _senderEmail;
            var senderPassword = _password;
            var smtpHost = _smtpServer;
            var smtpPort = _port;

            using (var smtpClient = new SmtpClient(smtpHost))
            {
                smtpClient.Port = smtpPort;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);
                await smtpClient.SendMailAsync(mailMessage);
            }
        }

    }
}

