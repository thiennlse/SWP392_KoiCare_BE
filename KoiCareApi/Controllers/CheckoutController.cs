using BusinessObject.Models;
using BusinessObject.RequestModel;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Service.Interface;
using System.Collections.Generic;

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

        public CheckoutController(PayOS payOS, IPaymentService paymentService, IOrderService orderService, IProductService productService)
        {
            _payOS = payOS;
            _paymentService = paymentService;
            _orderService = orderService;
            _productService = productService;
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
                    Description = string.Join(",",productName),
                    Status = "Đã thanh toán"
                };

                await _orderService.AddNewOrder(order);

                return Ok(paymentResult.checkoutUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
