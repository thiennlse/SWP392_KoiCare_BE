using Microsoft.Extensions.Configuration;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _senderEmail;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            // Reading SMTP settings from appsettings.json
            _smtpServer = configuration["SMTP:smtpServer"];
            _port = int.Parse(configuration["SMTP:port"]);
            _senderEmail = configuration["SMTP:senderEmail"];
            _password = configuration["SMTP:password"];
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {


            using (var client = new SmtpClient(_smtpServer, _port)
            {
                Credentials = new NetworkCredential(_senderEmail, _password),
                EnableSsl = true
            })
            using (var mail = new MailMessage(_senderEmail, recipientEmail)
            {
                Subject = subject,
                Body = body
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
                }
            }
        }
    }
}
