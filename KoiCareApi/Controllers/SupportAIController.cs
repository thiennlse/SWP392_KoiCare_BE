using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Service.Interface;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportAIController : ControllerBase
    {
        private readonly IGeminiService _opentAiService;
        private readonly IPoolService _poolService;
        private readonly IFishService _fishService;

        public SupportAIController(IGeminiService openAiService, IPoolService poolService, IFishService fishService)
        {
            _opentAiService = openAiService;
            _poolService = poolService;
            _fishService = fishService;
        }


        [HttpPost("ask-question/{poolId}")]
        public async Task<IActionResult> AskQuestion(int poolId) 
        {
            try {
                Pool pool = await _poolService.GetPoolById(poolId);
                if (pool == null) 
                { 
                return NotFound("pool is not exits");
                }
               var result = await _opentAiService.AskQuestion(pool);
                return Ok(result);
               
            }catch(Exception ex) {
            return BadRequest($"{ex.Message}");
            }
        }

       
        [HttpPost("supportcalculate/{fishId}")]
        public async Task<IActionResult> AskQuestionFishFood(int fishId) 
        {
            try
            {
               Fish fish = await _fishService.GetFishById(fishId);
                if(fish == null)
                {
                    return NotFound("fish is not exits");
                }
                var result = await _opentAiService.AskQuestionFishFood(fish);
                return Ok(result);
            }
            catch (Exception ex) {
                return BadRequest($"{ex.Message}");
            }
        }

    }
}
