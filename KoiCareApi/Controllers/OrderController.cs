using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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
            order.FoodId = _order.FoodId;
            order.PoolId = _order.PoolId;
            order.Name = _order.Name;
            order.Image = _order.Image;
            order.Size = _order.Size;
            order.Weight = _order.Weight;
            order.Age = _order.Age;
            order.Gender = _order.Gender;
            order.Origin = _order.Origin;
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
            order.Id = id;
            order.FoodId = _order.FoodId;
            order.PoolId = _order.PoolId;
            order.Name = _order.Name;
            order.Image = _order.Image;
            order.Size = _order.Size;
            order.Weight = _order.Weight;
            order.Age = _order.Age;
            order.Gender = _order.Gender;
            order.Origin = _order.Origin;

            await _orderService.UpdateById(order);
            return Ok(order);
        }
    }
}
