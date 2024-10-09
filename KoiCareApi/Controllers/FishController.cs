using BusinessObject.Models;
using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using System.Reflection.Metadata;
using Validation_Handler;
namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FishController : ControllerBase
    {
        private readonly IFishService _fishService;
        private readonly IUploadImage _uploadImage;

        public FishController(IFishService fishService,IUploadImage uploadImage)
        {
            _fishService = fishService;
            _uploadImage = uploadImage;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFish() 
        {
        var fish = await _fishService.GetAllFish();
            if (fish == null) 
            {
            return NotFound("empty Fish");
            }
            return Ok(fish);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFishById(int id) 
        {
            FishResponseModel fishResponse = new FishResponseModel();
            if (id <= 0) 
            {
                return BadRequest("phease input id >0");
            }
            var _fish = await _fishService.GetFishById(id);
            fishResponse.Size = _fish.Size;
            fishResponse.FoodId = _fish.FoodId;
            fishResponse.PoolId = _fish.PoolId;
            fishResponse.Age = _fish.Age;
            fishResponse.Weight = _fish.Weight;
            fishResponse.Origin = _fish.Origin;
            fishResponse.Gender = _fish.Gender;
            fishResponse.Image = _fish.Image;
            fishResponse.Food = _fish.Food;
            if (_fish == null) {
                return NotFound("fish is not exit");
            }
            return Ok(fishResponse);
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Tệp không hợp lệ.");
            }

            var imageURL = await _uploadImage.SaveImage(file);

            return Ok(new { Url = imageURL });
        }

        [HttpPost("add")]
        public async  Task<IActionResult> AddNewFish([FromBody] FishRequestModel _fish)
        {
            if (_fish == null) 
            { 
            return BadRequest("please input fish information");
            }
            Fish fish = new Fish();
           fish.FoodId = _fish.FoodId;
            fish.PoolId = _fish.PoolId;
            fish.Name = _fish.Name;
            fish.Image = _fish.Image; /*Validation_Handler.SaveImageToCloudinary.SaveImage(_fish_image);*/
            fish.Size = _fish.Size;
            fish.Weight = _fish.Weight;
            fish.Age = _fish.Age;
            fish.Gender = _fish.Gender;
            fish.Origin = _fish.Origin;
            await _fishService.AddNewFish(fish);
            return Created("Created", fish);
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
            var fish = await _fishService.GetFishById(id);
            if (_fish == null)
            {
                return NotFound("fish is not exits");
            }
            fish.Id = id;
            fish.FoodId = _fish.FoodId;
            fish.PoolId = _fish.PoolId;
            fish.Name = _fish.Name;
            fish.Image = _fish.Image;
            fish.Size = _fish.Size;
            fish.Weight = _fish.Weight;
            fish.Age = _fish.Age;
            fish.Gender = _fish.Gender;
            fish.Origin = _fish.Origin;

             await _fishService.UpdateById(fish);
            return Ok(fish);
        }
    }
}
