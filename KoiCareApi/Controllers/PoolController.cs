using BusinessObject.Models;
using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.Transactions;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoolController : ControllerBase
    {
        private readonly IPoolService _poolService;
        private readonly IWaterService _waterService;

        public PoolController(IPoolService poolService, IWaterService waterService)
        {
            _poolService = poolService;
            _waterService = waterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPoolAsync(int page = 1, int pageSize = 10, String? searchTerm = null)
        {
            var pool = await _poolService.GetAllPoolAsync(page, pageSize, searchTerm);
            if (pool == null)
            {
                return NotFound("empty Pool");
            }
            return Ok(pool);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPoolById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("phease input id >0");
            }
            var _pool = await _poolService.GetPoolById(id);
            if (_pool == null)
            {
                return NotFound("pool is not exit");
            }
            return Ok(_pool);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewPool([FromBody] PoolRequestModel _pool)
        {
            try
            {
                await _poolService.AddNewPool(_pool);
                return Ok("Create Successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var _pool = await _poolService.GetPoolById(id);
            if (_pool == null)
            {
                return NotFound("pool is not exits");
            }
            await _poolService.DeletePool(id);
            return NoContent();
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateById([FromBody] PoolRequestModel _pool, int id)
        {
            try
            {
                await _poolService.UpdatePool(id, _pool);
                return Ok("Updated Succesfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        [HttpGet("check-water-element-of-pool/{id}")]
        public async Task<IActionResult> ProductPecommendationByPoolId(int id)
        {
            try
            {
                WaterElementResponseModel productPecommentdation = await _poolService.CheckWaterElementInPool(id);

                return Ok(productPecommentdation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpGet("check-number-of-fish-in-pool/{id}")]
        public async Task<IActionResult> CheckNumberOfFishCanHave(int id)
        {
            try
            {
                int result = await _poolService.TotalFishInPool(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
