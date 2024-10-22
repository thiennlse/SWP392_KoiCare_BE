using Net.payOS.Types;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PaymentService : IPaymentService
    {
        
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
           
        }

        public async Task<CreatePaymentResult> createPaymentLink(PaymentData paymentData)
        {
            return await _paymentRepository.createPaymentLink(paymentData);
        }

        public async Task<PaymentLinkInformation> getPaymentLinkInformation(int id)
        {
            return await _paymentRepository.getPaymentLinkInformation(id);
        }

        public async Task<PaymentLinkInformation> cancelPaymentLink(int id, string reason)
        {
            return await _paymentRepository.cancelPaymentLink(id, reason);
        }

        public async Task<string> confirmWebhook(string url)
        {
            return await _paymentRepository.confirmWebhook(url);
        }

        public WebhookData verifyPaymentWebhookData(WebhookType webhookType)
        {
            return _paymentRepository.verifyPaymentWebhookData(webhookType);
        }

       
    }
}
