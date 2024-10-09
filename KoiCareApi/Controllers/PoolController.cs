using BusinessObject.Models;
using BusinessObject.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
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
        public async Task<IActionResult> GetAllPool()
        {
            var pool = await _poolService.GetAllPool();
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
                return BadRequest("please input id >0");
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
            if (_pool == null)
            {
                return BadRequest("please input pool information");
            }
            Pool pool = new Pool();
            pool.MemberId = _pool.MemberId;
            pool.Name = _pool.Name;
            pool.Size = _pool.Size;
            pool.Depth = _pool.Depth;
            pool.Description = _pool.Description;
            pool.WaterId = _pool.WaterId;
            await _poolService.AddNewPool(pool);
            return Created("Created", pool);
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
            var pool = await _poolService.GetPoolById(id);
            if (_pool == null)
            {
                return NotFound("pool is not exits");
            }

            pool.Id = id;
            pool.MemberId = _pool.MemberId;
            pool.Name = _pool.Name;
            pool.Size = _pool.Size;
            pool.Depth = _pool.Depth;
            pool.Description = _pool.Description;
            pool.WaterId = _pool.WaterId;

            await _poolService.UpdatePool(pool);
           
            return Ok(pool);
        }
        
        
        [HttpGet("CalculateSaltInPool/{id}")]
        public async Task<IActionResult> CalculateSaftInPool(int id) 
        {
           
           Pool _pool = await _poolService.GetPoolById(id);
            if(_pool == null)
            {
                return NotFound("this pool is no exits");
            }

            double volumeCubicMeters = _pool.Size * _pool.Depth;
            double volume = volumeCubicMeters * 1000;
           
            var  WaterOfPool = await _waterService.GetById(_pool.WaterId);

           double result = 0.001 *volume;
            if (WaterOfPool.Salt > result) { 
            return Ok(" Need to descrease");
            }
            if (WaterOfPool.Salt <result)
            {
                return Ok(" Need to increase");
            }
            return Ok("ok");
        }
    }
}
