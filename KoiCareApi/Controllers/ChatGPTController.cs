using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using ChatGPT.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace KoiCareApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatGPTController : ControllerBase
    {
        
        private readonly ILogger<ChatGPTController> _logger;
        private IConfiguration _configuration;

        public ChatGPTController(ILogger<ChatGPTController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("version")]
        public string Version()
        {
            return _configuration["VERSION"];
        }

        [HttpPost("fixGrammar")]
        public async Task<IActionResult> FixGrammar([FromBody] ChatGPTRequestModel request)
        {
            // retrieve ai key from configuration
            var openAiKey = _configuration["OPENAI_API_KEY"];

            if (openAiKey == null)
            {
                return NotFound("key not found");
            }

            var openai = new ChatGpt(openAiKey);

            var fixedSentence = await openai.Ask($"Fix the following sentence for spelling and grammar: {request.userInput}");
            if (fixedSentence == null)
            {
                return NotFound("unable to call chat gpt");
            }

            return Ok(new ChatGPTResponseModel() { Message = fixedSentence });
        }
    }
}
