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
        public async Task<IActionResult> Checkout([FromBody] PaymentRequestModel orderRequest)
        {
            try
            {
                List<ItemData> items = new List<ItemData>();
                int ordercode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));


                List<int> productList = orderRequest.orderId.ToList();
                foreach (int id in productList)
                {
                    Product product = await _productService.GetProductById(id);
                    ItemData item = new ItemData(product.Name, 1, (int)orderRequest.TotalCost);
                    items.Add(item);
                }
                
                PaymentData paymentData = new PaymentData(ordercode, (int)orderRequest.TotalCost, "payment order", items, orderRequest.cancelUrl, orderRequest.returnUrl);

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
