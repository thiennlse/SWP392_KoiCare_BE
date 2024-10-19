using BusinessObject.Models;
using BusinessObject.RequestModel;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Service.Interface;

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
                int ordercode = int.Parse(DateTimeOffset.Now.ToString("ffffff")); // Tạo mã đơn hàng
                int totalAmount = 0;

                foreach (var request in orderRequest)
                {
                    Product product = await _productService.GetProductById(request.ProductId);

                    if (product == null)
                    {
                        return BadRequest($"Product or order not found for orderId {request.ProductId}");
                    }

                    // Tính tổng giá cho mỗi sản phẩm
                    int price = (int)(request.Cost * request.quantity);
                    totalAmount += price;
                    // Tạo ItemData cho từng sản phẩm
                    ItemData item = new ItemData(product.Name, request.quantity, price);
                    items.Add(item);
                }

                // Chuẩn bị dữ liệu thanh toán với tổng số tiền
                PaymentData paymentData = new PaymentData
                (
                    ordercode,
                    totalAmount,
                    "payment order",
                    items,
                    cancelUrl,
                    returnUrl
                );

                // Truyền tổng số tiền vào PaymentData

                // Tạo đường dẫn thanh toán
                CreatePaymentResult paymentResult = await _paymentService.createPaymentLink(paymentData);

                return Ok(paymentResult.checkoutUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
