using BusinessObject.Models;
using BusinessObject.RequestModel;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Validation.Food;


namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private  readonly IFoodService _foodService;
        private   readonly FoodValidation _validation;
        public FoodController(IFoodService foodService, FoodValidation validations)
        {
            _foodService = foodService;
            _validation = validations;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFoodAsync(int page = 1, int pageSize = 10, string? searchTerm = null)
        {

            var _food = await _foodService.GetAllFoodAsync(page, pageSize, searchTerm);
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
            try
            {
                ValidationResult validationResult = _validation.Validate(_food);
                if (validationResult.IsValid) 
              {
                    Food food = new Food
                    {
                       Name = _food.Name,
                       Weight = _food.Weight,  
                    };
                    await _foodService.AddNewFood(food);
                    return Created("create", food);
              }
                var errors = validationResult.Errors.Select(e => (object)new
                {
                    e.PropertyName,
                    e.ErrorMessage

                }).ToList();
                return BadRequest(errors);
            }
            catch (Exception ex) 
            {
            return BadRequest(ex.Message); 
            }
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
            try
            {
                ValidationResult validationResult = _validation.Validate(_food);
                var errors = validationResult.Errors.Select(e => (object)new
                {
                    e.PropertyName,
                    e.ErrorMessage
                }).ToList();

                if (validationResult.IsValid)
                { 
                Food food = await _foodService.GetFoodById(id);
                    if (food != null) {
                        food.Id = id;
                        food.Name = _food.Name;
                        food.Weight = _food.Weight;
                        await _foodService.UpdateFood(food);
                        return Ok(food);
                    }
                }
                return BadRequest(errors);
            }
            catch (Exception ex) {
            return BadRequest(ex.Message);
            }
        }

    }
}
