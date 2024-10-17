using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IPaymentService
    {
        public Task<CreatePaymentResult> createPaymentLink(PaymentData paymentData);
        
        public  Task<PaymentLinkInformation> getPaymentLinkInformation(int id);

        public Task<PaymentLinkInformation> cancelPaymentLink(int id, string reason);

        public Task<string> confirmWebhook(string url);

        public WebhookData verifyPaymentWebhookData(WebhookType webhookType);
    }
}
