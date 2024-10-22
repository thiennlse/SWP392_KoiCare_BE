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
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _paymentService.getPaymentLinkInformation(orderCode);

                string body = $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 20px;'>
    <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #ffffff; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
      <p style='font-size: 16px;'>Xin chào,</p>
      <p style='font-size: 16px;'>Đây là hóa đơn mua hàng của bạn:</p>
      <table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>
        <tr style='background-color: #f8f8f8;'>
          <td style='padding: 12px; border: 1px solid #ddd; font-weight: bold;'>Mã đơn hàng:</td>
          <td style='padding: 12px; border: 1px solid #ddd;'>{paymentLinkInformation.orderCode}</td>
        </tr>
        <tr>
          <td style='padding: 12px; border: 1px solid #ddd; font-weight: bold;'>Ngày thanh toán:</td>
          <td style='padding: 12px; border: 1px solid #ddd;'>{paymentLinkInformation.createdAt:yyyy-MM-dd HH:mm:ss}</td>
        </tr>
        <tr style='background-color: #f8f8f8;'>
          <td style='padding: 12px; border: 1px solid #ddd; font-weight: bold;'>Tổng tiền:</td>
          <td style='padding: 12px; border: 1px solid #ddd;'>{paymentLinkInformation.amountPaid:#,##0} ₫</td>
        </tr>
        <tr>
          <td style='padding: 12px; border: 1px solid #ddd; font-weight: bold;'>Trạng thái thanh toán:</td>
          <td style='padding: 12px; border: 1px solid #ddd;'>{paymentLinkInformation.status}</td>
        </tr>
      </table>
      <p style='font-size: 16px; margin-top: 20px;'>Cám ơn đã sử dụng dịch vụ của chúng tôi</p>
    </div>
  </body>
</html>";


                if (emailRequest == null || string.IsNullOrEmpty(emailRequest.RecipientEmail))
                {
                    return BadRequest("invalid email Request");
                }


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
