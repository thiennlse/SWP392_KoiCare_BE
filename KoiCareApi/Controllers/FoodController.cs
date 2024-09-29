using BusinessObject.Models;
using BusinessObject.RequestModel;
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
        public async Task<IActionResult> AddNewFood([FromBody] FoodRequestModel _food)
        {
            if (_food == null)
            {
                return BadRequest("please input  Food information");
            }

            var food = new Food();
            food.Name = _food.Name; 
            food.Weight = _food.Weight;
            
            await   _foodService.AddNewFood(food);
            return Created("created",food);

        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var _food = await _foodService.GetFoodById(id);
            if (_food == null) 
            {
            return NotFound("food is no exits");
            }
            await _foodService.DeleteFood(id);
            return NoContent();
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateById([FromBody]FoodRequestModel _food ,int id) 
        {
        var food = await _foodService.GetFoodById(id);
            if (food == null) 
            {
            return NotFound("this food is not exits");
            }
            food.Id = id;
            food.Name = _food.Name;
            food.Weight = _food.Weight;


            await _foodService.UpdateFood(food);
            return Ok(food);
        }

    }
}
