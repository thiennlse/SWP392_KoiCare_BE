using BusinessObject.Models;
using BusinessObject.RequestModel;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Validation_Handler.FoodValidation;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private  readonly IFoodService _foodService;
        private readonly   IFishService _fishService;
        private readonly FoodValidation _foodValidation;
        public FoodController(IFoodService foodService, IFishService fishService, FoodValidation foodValidation)
        {
            _foodService = foodService;
            _fishService = fishService;
            _foodValidation = foodValidation;
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
           ValidationResult validationResult = _foodValidation.Validate(_food);

            if (!validationResult.IsValid) 
            {
                return BadRequest(validationResult.ToString());
            }

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

            ValidationResult validationResult = _foodValidation.Validate(_food);

            if (!validationResult.IsValid) 
            {
                return BadRequest(validationResult.ToString() );
            }

            food.Id = id;
            food.Name = _food.Name;
            food.Weight = _food.Weight;


            await _foodService.UpdateFood(food);
            return Ok(food);
        }

        [HttpGet("CalculateFood/{id}")]
        public async Task<IActionResult> CalculateFoodForFish(int id)
        {
            double result = 0;
            result = await _foodService.CalculateFishFood(id);
            Fish _fish = new Fish();
            _fish = await _fishService.GetFishById(id);
            
            if(_fish.Food.Weight > result)
            {
                return Ok("need to descrease");

            }
            if(_fish.Food.Weight < result)
            {
                return Ok("need to increase");
            }
            return Ok("ok");

        }

    }
}
