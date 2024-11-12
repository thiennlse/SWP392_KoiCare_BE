using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using ChatGPT.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Interface;

namespace KoiCareApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatGPTController : ControllerBase
    {
        
        private readonly ILogger<ChatGPTController> _logger;
        private IConfiguration _configuration;
        private readonly IChatGPTService _chatGPTService;



        public ChatGPTController(ILogger<ChatGPTController> logger, IConfiguration configuration, IChatGPTService chatGPTService)
        {
            _logger = logger;
            _configuration = configuration;
            _chatGPTService = chatGPTService;
            
        }

        [HttpGet("version")]
        public string Version()
        {
            return _configuration["VERSION"];
        }

        [HttpPost("fixGrammar")]
        public async Task<IActionResult> FixGrammarAsync([FromBody] ChatGPTRequestModel request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.userInput))
            {
                return BadRequest("Invalid request data. 'userInput' cannot be empty.");
            }

            try
            {
                // Process the text using the ChatGPT service
                var fixedGrammarText = await _chatGPTService.ProcessGrammarFix(
                    request.userInput,
                    request.Model,
                    request.Temperature,
                    request.MaxTokens
                );

                // Return the corrected text in the response
                return Ok(new { correctedText = fixedGrammarText });
            }
            catch (Exception ex)
            {
                // Log the error if necessary
                Console.WriteLine($"Error: {ex.Message}");

                // Return a generic error message to the client
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    }
}
