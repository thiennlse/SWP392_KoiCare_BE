using BusinessObject.Models;
using BusinessObject.RequestModel;
using CloudinaryDotNet.Actions;
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
        private readonly OrderValidation _orderValidate;

        public OrderController(OrderValidation orderValidate, IOrderService orderService, IProductService productService, IMemberService memberService, IPaymentService paymentService)
        {

            _orderService = orderService;
            _productService = productService;
            _memberService = memberService;
            _paymentService = paymentService;
            _orderValidate = orderValidate;
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
                ValidationResult validationResult = _orderValidate.Validate(_order);
                if (validationResult.IsValid)
                {
                    await _orderService.AddNewOrder(_order);
                    return Ok("Created");
                }
                var error = validationResult.Errors;
                return BadRequest(error);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
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
                ValidationResult validationResult = _orderValidate.Validate(_order);
                if (validationResult.IsValid)
                {
                    await _orderService.UpdateOrder(id, _order);
                    return Ok("Update Successful");
                }
                var error = validationResult.Errors;
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("order-date-range")]
        public async Task<IActionResult> GetOrdersByOrderDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime closeDate)
        {
            try
            {
                // Call the service to get orders within the date range
                var orders = await _orderService.GetOrdersByOrderDateRange(startDate, closeDate);
                return Ok(orders); // Return the filtered orders
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("close-date-range")]
        public async Task<IActionResult> GetOrdersByCloseDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime closeDate)
        {
            try
            {
                // Call the service to get orders within the date range
                var orders = await _orderService.GetOrdersByCloseDateRange(startDate, closeDate);
                return Ok(orders); // Return the filtered orders
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("search-by-dates")]
        public async Task<IActionResult> GetOrdersByOrderDateAndCloseDate([FromQuery] DateTime startOrderDate, [FromQuery] DateTime endOrderDate, [FromQuery] DateTime startCloseDate, [FromQuery] DateTime endCloseDate)
        {
            try
            {
                // Call the service method
                var orders = await _orderService.GetOrdersByOrderDateAndCloseDate(startOrderDate, endOrderDate, startCloseDate, endCloseDate);

                // If no orders are found, return 404 Not Found
                if (orders == null || !orders.Any())
                {
                    return NotFound(new { message = "No orders found within the specified date ranges." });
                }

                // Return the list of filtered orders with 200 OK
                return Ok(orders);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return a 500 Internal Server Error
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpGet("search-by-member/{id}")]
        public async Task<IActionResult> SearchOrdersByUserId(int id, int page = 1, int pageSize = 100, string? searchTerm = null)
        {
            try
            {
                // Call the service method
                var orders = await _orderService.SearchOrdersByUserId(id, page, pageSize, searchTerm);

                // If no orders are found, return 404 Not Found
                if (orders == null || !orders.Any())
                {
                    return NotFound(new { message = "No orders found for the specified user." });
                }

                // Return the list of filtered orders with 200 OK
                return Ok(orders);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return a 500 Internal Server Error
                return StatusCode(500, new { message = "An error occurred while processing your request.", details = ex.Message });
            }
        }
    }
}
