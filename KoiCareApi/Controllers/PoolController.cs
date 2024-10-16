﻿using BusinessObject.Models;
using BusinessObject.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoolController : ControllerBase
    {
        private readonly IPoolService _poolService;

        public PoolController(IPoolService poolService)
        {
            _poolService = poolService;
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

            pool.MemberId = _pool.MemberId;
            pool.Name = _pool.Name;
            pool.Size = _pool.Size;
            pool.Depth = _pool.Depth;
            pool.Description = _pool.Description;
            pool.WaterId = _pool.WaterId;

            await _poolService.UpdatePool(pool);
            return Ok(pool);
        }
    }
}
