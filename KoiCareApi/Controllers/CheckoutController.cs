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


        public CheckoutController(PayOS payOS, IPaymentService paymentService, IOrderService orderService, IProductService productService, IEmailService emailService)
        {
            _payOS = payOS;
            _paymentService = paymentService;
            _orderService = orderService;
            _productService = productService;
            _emailService = emailService;
        }

        [HttpPost("create-payment-link")]
        public async Task<IActionResult> Checkout([FromBody] List<PaymentRequestModel> orderRequest, string cancelUrl, string returnUrl)
        {
            try
            {
                List<ItemData> items = new List<ItemData>();
                int ordercode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                int totalAmount = 0;
                List<int> productids = new List<int>();
                List<string> productName = new List<string>();
                foreach (var request in orderRequest)
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
                    product.InStock -= request.quantity;
                    productids.Add(product.Id);
                    productName.Add(product.Name);
                    int price = (int)(request.Cost * request.quantity);
                    totalAmount += price;
                    ItemData item = new ItemData(product.Name, request.quantity, price);
                    items.Add(item);
                }
                PaymentData paymentData = new PaymentData
                (
                    ordercode,
                    totalAmount,
                    "payment order",
                    items,
                    cancelUrl,
                    returnUrl
                );
                CreatePaymentResult paymentResult = await _paymentService.createPaymentLink(paymentData);

                OrderRequestModel order = new OrderRequestModel
                {
                    ProductId = productids,
                    TotalCost = totalAmount,
                    CloseDate = DateTime.Now,
                    Code = paymentResult.orderCode.ToString(),
                    Description = string.Join(",", productName),
                    Status = "Chưa thanh toán"
                };

                await _orderService.AddNewOrder(order);

                return Ok(new
                {
                    Url = paymentResult.checkoutUrl,
                    orderCode = paymentResult.orderCode

                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("send/{orderCode}")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequestModel emailRequest, int orderCode)
        {
            PaymentLinkInformation paymentLinkInformation = await _paymentService.getPaymentLinkInformation(orderCode);

            string body = $@"
        <html>
            <body style='font-family: Arial, sans-serif;'>
                 <p>Hi,</p>
                 <p>Here are the details of your payment:</p>
            <table style='border-collapse: collapse;'>
                     <tr>
                     <td style='padding: 8px;'><strong>Order Code:</strong></td>
                     <td style='padding: 8px;'>{paymentLinkInformation.orderCode}</td>
                     </tr>
                     <tr>
                     <td style='padding: 8px;'><strong>Created At:</strong></td>
                    <td style='padding: 8px;'>{paymentLinkInformation.createdAt:yyyy-MM-dd HH:mm:ss}</td>
                     </tr>
                     <tr>
                   <td style='padding: 8px;'><strong>Amount Paid:</strong></td>
                  <td style='padding: 8px;'>{paymentLinkInformation.amountPaid:C}</td>
                 </tr>
                <tr>
                 <td style='padding: 8px;'><strong>Status:</strong></td>
                  <td style='padding: 8px;'>{paymentLinkInformation.status}</td>
             </tr>
                 </table>
                <p>Thank you for your payment!</p>
                </body>
                 </html>";

            if (emailRequest == null || string.IsNullOrEmpty(emailRequest.RecipientEmail))
            {
                return BadRequest("invalid email Request");
            }

            try
            {
                await _emailService.SendEmailAsync(emailRequest.RecipientEmail, emailRequest.Subject, body);
                return Ok("Email sent");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
