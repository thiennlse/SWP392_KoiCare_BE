using BusinessObject.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace KoiCareApi.Controllers
{
    [Route("api/subcriptions")]
    [ApiController]
    public class SubcriptionsController : ControllerBase
    {
        private readonly ISubcriptionService _subcriptionService;

        public SubcriptionsController(ISubcriptionService subcriptionService)
        {
            _subcriptionService = subcriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var subcriptions = await _subcriptionService.GetAll();
                return Ok(subcriptions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var subcription = await _subcriptionService.GetById(id);
                return Ok(subcription);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(SubcriptionRequest request)
        {
            try
            {
                await _subcriptionService.Add(request);
                return Ok("Created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SubcriptionRequest request)
        {
            try
            {
                await _subcriptionService.Update(id, request);
                return Ok("Updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _subcriptionService.DeleteById(id);
                return Ok("Delete successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
