using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using Repository.Interface;
namespace Repository
{
    public class PaymentRepository : IPaymentRepository
    {

        public async Task<CreatePaymentResult> createPaymentLink(PaymentData paymentData)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            
            IConfiguration configuration = builder.Build();

            var client_id = configuration["Environment:PAYOS_CLIENT_ID"];
            var api_key = configuration["Environment:PAYOS_API_KEY"];
            var checkSum_key = configuration["Environment:PAYOS_CHECKSUM_KEY"];

            PayOS payOS = new PayOS(client_id, api_key, checkSum_key);

            PaymentData payment = new PaymentData (
                paymentData.orderCode,
                paymentData.amount,
                "Payment Order",
                paymentData.items,
                paymentData.cancelUrl,
                paymentData.returnUrl
                );

            CreatePaymentResult createPayment = await payOS.createPaymentLink( paymentData );
            return createPayment;

        }


        public async Task<PaymentLinkInformation> getPaymentLinkInformation(int id)
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            var client_id = configuration["Environment:PAYOS_CLIENT_ID"];
            var api_key = configuration["Environment:PAYOS_API_KEY"];
            var checkSum_key = configuration["Environment:PAYOS_CHECKSUM_KEY"];

            PayOS payOS = new PayOS(client_id, api_key, checkSum_key);
            PaymentLinkInformation paymentLinkInformation = await payOS.getPaymentLinkInformation(id);
            return paymentLinkInformation;
        }

        public async Task<PaymentLinkInformation> cancelPaymentLink(int id, string reason) 
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            var client_id = configuration["Environment:PAYOS_CLIENT_ID"];
            var api_key = configuration["Environment:PAYOS_API_KEY"];
            var checkSum_key = configuration["Environment:PAYOS_CHECKSUM_KEY"];

            PayOS payOS = new PayOS(client_id, api_key, checkSum_key);

            PaymentLinkInformation cancelledPaymentLinkInfo = await  payOS.cancelPaymentLink(id, reason);
            return cancelledPaymentLinkInfo;
        }

        public async Task<string> confirmWebhook(string url) 
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            var client_id = configuration["Environment:PAYOS_CLIENT_ID"];
            var api_key = configuration["Environment:PAYOS_API_KEY"];
            var checkSum_key = configuration["Environment:PAYOS_CHECKSUM_KEY"];

            PayOS payOS = new PayOS(client_id, api_key, checkSum_key);
             return await payOS.confirmWebhook(url);
            
        }



        public WebhookData verifyPaymentWebhookData(WebhookType webhookType) 
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            var client_id = configuration["Environment:PAYOS_CLIENT_ID"];
            var api_key = configuration["Environment:PAYOS_API_KEY"];
            var checkSum_key = configuration["Environment:PAYOS_CHECKSUM_KEY"];

            PayOS payOS = new PayOS(client_id, api_key, checkSum_key);
            WebhookData webhookData = payOS.verifyPaymentWebhookData(webhookType);
            return webhookData;

        }
    }
}
