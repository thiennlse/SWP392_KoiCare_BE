using BusinessObject.Models;
using BusinessObject.RequestModel;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.Interface;
using System.Reflection.Metadata;
using Validation.Fish;
namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FishController : ControllerBase
    {
        private readonly IFishService _fishService;
        private readonly IUploadImage _uploadImage;
        private readonly FishValidation _validation;
        private readonly IFoodService _foodService;
        public FishController(IFishService fishService, IUploadImage uploadImage, FishValidation validation, IFoodService foodService)
        {
            _fishService = fishService;
            _uploadImage = uploadImage;
            _validation = validation;
            _foodService = foodService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFishAsync(int page = 1, int pageSize = 10, string? searchTerm = null)
        {
        var fish = await _fishService.GetAllFishAsync(page, pageSize, searchTerm);
            if (fish == null) 
            {
            return NotFound("empty Fish");
            }
            return Ok(fish);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFishById(int id) 
        {
            if (id <= 0) 
            {
                return BadRequest("phease input id >0");
            }
            var _fish = await _fishService.GetFishById(id);
            if (_fish == null) {
                return NotFound("fish is not exit");
            }
            return Ok(_fish);
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Not a valid file");
            }

            var imageURL = await _uploadImage.SaveImage(file);

            return Ok(new { Url = imageURL });
        }

        [HttpPost("add")]
        public async  Task<IActionResult> AddNewFish([FromBody] FishRequestModel _fish)
        {
            try
            {
                ValidationResult validationResult = _validation.Validate(_fish);
                if (validationResult.IsValid)
                {
                    Fish fish = new Fish
                    {
                        FoodId = _fish.FoodId,
                        PoolId = _fish.PoolId,
                        Name = _fish.Name,
                        Image = _fish.Image,
                        Size = _fish.Size,
                        Weight = _fish.Weight,
                        Dob = _fish.Dob,
                        Gender = _fish.Gender,
                        Origin = _fish.Origin
                    };
                    await _fishService.AddNewFish(fish);
                    return Created("Created", fish);
                }
                var errors = validationResult.Errors.Select(e => (object)new
                {
                    e.PropertyName,
                    e.ErrorMessage
                }).ToList();
                return BadRequest(errors);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteById(int id) 
        {
            var _fish = await _fishService.GetFishById(id);
            if (_fish == null) 
            {
            return NotFound("fish is not exits");
            }
            await _fishService.DeleteFish(id);
            return NoContent();
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateById([FromBody]FishRequestModel _fish, int id) 
        {
            try
            {
                ValidationResult validationResult = _validation.Validate(_fish);
                var errors = validationResult.Errors.Select(e => (object)new
                {
                    e.PropertyName,
                    e.ErrorMessage
                }).ToList();
                if (validationResult.IsValid)
                {
                    var fish = await _fishService.GetFishById(id);
                    if (fish != null)
                    {
                        fish.Id = id;
                        fish.FoodId = _fish.FoodId;
                        fish.PoolId = _fish.PoolId;
                        fish.Name = _fish.Name;
                        fish.Image = _fish.Image;
                        fish.Size = _fish.Size;
                        fish.Weight = _fish.Weight;
                        fish.Dob = _fish.Dob;
                        fish.Gender = _fish.Gender;
                        fish.Origin = _fish.Origin;
                        await _fishService.UpdateById(fish);
                        return Ok(fish);
                    }
                }
                return BadRequest(errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("calculateFoodFish/{id}")]
        public async Task<IActionResult> CalculateFoodForFish(int id) {
            if (id <= 0)
            {
                return BadRequest("fish id must greater than 0");

            }
            Fish fish =  await _fishService.GetFishById(id);
            if (fish == null) 
            {
            return NotFound("fish is not exits");
            }
            double foodStandardForFish = await _foodService.CalculateFishFood(id);
            
            Food foodOfFish = await _foodService.GetFoodById(fish.FoodId);

            double result = 0;
                
            if(foodOfFish.Weight > foodStandardForFish)
            {   // descrease food with result unit
                result = foodOfFish.Weight - foodStandardForFish;
                return Ok("descrease "+result);
            }
            if(foodOfFish.Weight < foodStandardForFish)
            {
                // increase food with result unit
                result = foodStandardForFish - foodOfFish.Weight;
                return Ok("increase " + result);
            }
            if (foodOfFish.Weight == result) 
            {
                result = foodStandardForFish;
                return Ok("keep " + result);
            }
            return Ok();
        
        }
    }
}
