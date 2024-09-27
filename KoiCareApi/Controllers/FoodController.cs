using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private  readonly IFoodService _foodService;

        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFood() {

            var _food = await _foodService.GetAllFood();
            if (_food == null)
            {
                return NotFound("Empty Food");
            }
            return Ok(_food);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodById(int id)
        {
            if (id <= 0) {
                return BadRequest("please input >0");
            }
            var _food = await _foodService.GetFoodById(id);
            if (_food == null) {
                return NotFound("no exits");
            }
            return Ok(_food);

        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewFood([FromBody] Food _food)
        {
            if (_food == null)
            {
                return BadRequest("please input  Food information");
            }
            await _foodService.AddNewFood(_food);
            return Ok("add successfully");

        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var _food = await _foodService.GetFoodById(id);
            if (_food == null) 
            {
            return NotFound("food is no exits");
            }
            await _foodService.DeleteFoodById(id);
            return Ok("delete successfully");
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateById([FromBody]Food _food) 
        {
        var food = await _foodService.GetFoodById(_food.Id);
            if (food == null) 
            {
            return NotFound("this food is not exits");
            }
            await _foodService.UpdateFoodById(_food);
            return Ok("update successfully");
        }

    }
}
