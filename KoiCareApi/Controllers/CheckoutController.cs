using BusinessObject.Models;
using BusinessObject.RequestModel;
using CloudinaryDotNet.Actions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Service;
using Service.Interface;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;


namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly PayOS _payOS;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IEmailService _emailService;
        private readonly ISubcriptionService _subcriptionService;

        public CheckoutController(PayOS payOS, IPaymentService paymentService, IOrderService orderService, IProductService productService, IEmailService emailService, ISubcriptionService subcriptionService)
        {
            _payOS = payOS;
            _paymentService = paymentService;
            _orderService = orderService;
            _productService = productService;
            _emailService = emailService;
            _subcriptionService = subcriptionService;
        }

        [HttpPost("create-payment-link")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest paymentRequest)
        {
            try
            {
                List<ItemData> items = new List<ItemData>();
                int ordercode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                int totalAmount = 0;
                PaymentData paymentData = null;
                if (paymentRequest.SubscriptionId != 0)
                {
                    if (!paymentRequest.SubscriptionId.HasValue)
                    {
                        return BadRequest("Subscription ID is required for subscription payment.");
                    }

                    var subscription = await _subcriptionService.GetById(paymentRequest.SubscriptionId.Value);
                    if (subscription == null)
                    {
                        return BadRequest("Subscription not found.");
                    }

                    totalAmount = (int)subscription.Price;
                    ItemData item = new ItemData(subscription.Name, 1, (int)subscription.Price);
                    items.Add(item);

                    paymentData = new PaymentData(
                        ordercode,
                        totalAmount,
                        $"{subscription.Name} Plan",
                        items,
                        paymentRequest.CancelUrl,
                        paymentRequest.ReturnUrl
                    );
                }
                else
                {
                    foreach (var request in paymentRequest.OrderRequest)
                    {
                        Product product = await _productService.GetProductById(request.ProductId);

                        if (product == null)
                        {
                            return BadRequest($"Product or order not found for orderId {request.ProductId}");
                        }
                        if (product.InStock < request.quantity)
                        {
                            return BadRequest("Not enough product");
                        }

                        int price = (int)(request.Cost * request.quantity);
                        totalAmount += price;
                        ItemData item = new ItemData(product.Name, request.quantity, price);
                        items.Add(item);
                    }

                    paymentData = new PaymentData(
                        ordercode,
                        totalAmount,
                        "payment order",
                        items,
                        paymentRequest.CancelUrl,
                        paymentRequest.ReturnUrl
                    );
                }
                CreatePaymentResult paymentResult = await _paymentService.createPaymentLink(paymentData);
                return Ok(paymentResult.checkoutUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("send-email/{orderCode}")]
        public async Task<IActionResult> SendOrderEmail([FromBody] EmailRequestModel emailRequest, string orderCode)
        {
            if (emailRequest == null || string.IsNullOrEmpty(emailRequest.RecipientEmail))
            {
                return BadRequest("Invalid email request.");
            }
            try
            {
                // Send the email asynchronously
                await _emailService.SendVerifyAccountEmail(emailRequest.RecipientEmail, orderCode);

                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

    }
}
