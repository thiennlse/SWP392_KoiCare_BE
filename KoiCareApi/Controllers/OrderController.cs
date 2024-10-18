using BusinessObject.Models;
using BusinessObject.RequestModel;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Validation.Order;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMemberService _memberService;
        private readonly IProductService _productService;
        private readonly IPaymentService _paymentService;
        private readonly OrderValidation _validation;
        public OrderController(IOrderService orderService, IProductService productService, IMemberService memberService,IPaymentService paymentService, OrderValidation validations)
        { 
           
            _orderService = orderService;
            _productService = productService;
            _memberService = memberService;
            _paymentService = paymentService;           
            _validation = validations;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderAsync(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
            var order = await _orderService.GetAllOrderAsync(page, pageSize, searchTerm);
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
            try
            {
                ValidationResult validationResult = _validation.Validate(_order);
                if (validationResult.IsValid) {
                    Order order = new Order
                    {
                        MemberId = _order.MemberId,
                        ProductId = _order.ProductId,
                        TotalCost = _order.TotalCost,
                        OrderDate = DateTime.Now,
                        CloseDate = _order.CloseDate,
                        Code = _order.Code,
                        Description = _order.Description,
                        Status = _order.Status,
                        Member = await _memberService.GetMemberById(_order.MemberId),
                        Product = await _productService.GetProductById(_order.ProductId)

                    };
                    await _orderService.AddNewOrder(order);
                    return Created("Created", order);
                }
                var errors = validationResult.Errors.Select(e => (object)new
                {
                    e.PropertyName,
                    e.ErrorMessage

                }).ToList();
                return BadRequest(errors);
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);
            }
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
            try
            {
                ValidationResult validationResult = _validation.Validate(_order);
                var errors = validationResult.Errors.Select(e => (object)new
                {
                    e.PropertyName,
                    e.ErrorMessage
                }).ToList();

                if (validationResult.IsValid) { 
                Order order = await _orderService.GetOrderById(id);
                    if (order != null) {
                    order.Id = id;
                    order.MemberId = _order.MemberId;
                    order.ProductId = _order.ProductId;
                    order.TotalCost = _order.TotalCost;
                    order.OrderDate = DateTime.Now;
                    order.CloseDate = _order.CloseDate;
                    order.Status = _order.Status;
                    order.Code = _order.Code;
                    order.Description = _order.Description;
                    order.Member = await _memberService.GetMemberById(_order.MemberId);
                    order.Product = await _productService.GetProductById(_order.ProductId);
                        await _orderService.UpdateOrder(order);
                        return Ok(order);
                    }
                }
                    return  BadRequest(errors);
            }
            catch (Exception ex) 
            {
            return BadRequest(ex.Message);
            }
                 
        }

       
    }
}
