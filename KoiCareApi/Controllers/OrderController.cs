using BusinessObject.Models;
using BusinessObject.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMemberService _memberService;
        private readonly IProductService _productService;
        public OrderController(IOrderService orderService, IProductService productService, IMemberService memberService)
        {
            _orderService = orderService;
            _productService = productService;
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrder()
        {
            var order = await _orderService.GetAllOrder();
            if (order == null)
            {
                return NotFound("empty Order");
            }
            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("phease input id >0");
            }
            var _order = await _orderService.GetOrderById(id);
            if (_order == null)
            {
                return NotFound("order is not exit");
            }
            return Ok(_order);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewOrder([FromBody] OrderRequestModel _order)
        {
            if (_order == null)
            {
                return BadRequest("please input order information");
            }
            Order order = new Order();
            
            order.MemberId = _order.MemberId;
            order.ProductId = _order.ProductId;
            order.TotalCost = _order.TotalCost;
            order.OrderDate = DateTime.Now;
            order.CloseDate = _order.CloseDate;
            order.Code = _order.Code;
            order.Description = _order.Description;
            order.Status = _order.Status;
            order.Member = await _memberService.GetMemberById(order.MemberId);
            order.Product = await _productService.GetProductById(order.ProductId);

            await _orderService.AddNewOrder(order);
            return Created("Created", order);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var _order = await _orderService.GetOrderById(id);
            if (_order == null)
            {
                return NotFound("order is not exits");
            }
            await _orderService.DeleteOrder(id);
            return NoContent();
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateById([FromBody] OrderRequestModel _order, int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (_order == null)
            {
                return NotFound("order is not exits");
            }
            
            order.MemberId = _order.MemberId;
            order.ProductId = _order.ProductId;
            order.TotalCost = _order.TotalCost;
            order.CloseDate = _order.CloseDate;
            order.Code = _order.Code;
            order.Description = _order.Description;
            order.Status = _order.Status;

            await _orderService.UpdateOrder(order);
            return Ok(order);
        }
    }
}
