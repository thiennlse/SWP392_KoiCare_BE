using BusinessObject.Models;
using BusinessObject.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterController : ControllerBase
    {
        private readonly IWaterService _waterService;

        public WaterController(IWaterService waterService)
        {
            _waterService = waterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var _water = await _waterService.GetAll();

            if (_water == null) { 
            return NotFound("water is empty");
            }
            return Ok(_water);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            if (id <= 0) 
            {
                return BadRequest("id is not <= 0");
            }
            var _water =  await _waterService.GetById(id);
            if (_water == null)
            {
                return NotFound("water is not exits");
            }
            return Ok(_water);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddWater([FromBody]WaterRequestModel _water)
        {
            if (_water == null) {
                return BadRequest("please input information");
            }
            Waters waters = new Waters();
            waters.Salt = _water.Salt;
            waters.Temperature = _water.Temperature;
            waters.Po4 = _water.Po4;
            waters.No3 = _water.No3;
            waters.No2 = _water.No2;
            waters.O2 = _water.O2;
            waters.Ph = _water.Ph;
            await _waterService.addWater(waters);
            return Created("Create", waters);

        }

        [HttpPatch("update/{id}")]
         public async Task<IActionResult> UpdateWater([FromBody]WaterRequestModel _water, int id)
        {
            var waters = await _waterService.GetById(id);
            if (waters == null) { 
            return NotFound(" water is not exit");  
            }
            waters.Id = id;
            waters.Salt = _water.Salt;
            waters.Temperature = _water.Temperature;
            waters.Po4 = _water.Po4;
            waters.No3 = _water.No3;
            waters.No2 = _water.No2;
            waters.O2 = _water.O2;
            waters.Ph = _water.Ph;
            await _waterService.updateWater(waters);
            return Ok(waters);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteWater(int id) 
        {
            var _water = await _waterService.GetById(id);
            if (_water == null)
            {
                return NotFound("this water is not exits");
            }
            await _waterService.deleteWater(id);
            return NoContent();
        }




        }

    }
