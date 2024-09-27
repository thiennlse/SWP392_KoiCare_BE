using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FishController : ControllerBase
    {
        private readonly IFishService _fishService;

        public FishController(IFishService fishService)
        {
            _fishService = fishService;
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

        [HttpPost("add")]
        public async  Task<IActionResult> AddNewFish([FromBody]Fish _fish)
        {
            if (_fish == null) 
            { 
            return BadRequest("please input fish information");
            }
             await _fishService.AddNewFish(_fish);
            return Ok("add successfully");
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteById(int id) 
        {
            var _fish = await _fishService.GetFishById(id);
            if (_fish == null) 
            {
            return NotFound("fish is not exits");
            }
            await _fishService.DeleteById(id);
            return Ok("delete scuccessfully");
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateById([FromBody]Fish _fish) 
        {
        var fish = await _fishService.GetFishById(_fish.Id);
            if (fish == null)
            {
                return NotFound("fish is not exits");
            }
            await _fishService.UpdateById(_fish);
            return Ok("update successfully");
        }
    }
}
