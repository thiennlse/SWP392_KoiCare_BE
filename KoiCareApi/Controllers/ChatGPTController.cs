using BusinessObject.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace KoiCareApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatGPTController : ControllerBase
    {
        private readonly ChatGPTClient _chatGptClient;

        public ChatGPTController(ChatGPTClient chatGptClient)
        {
            _chatGptClient = chatGptClient;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatGPTRequestModel request)
        {
            try
            {
                var response = await _chatGptClient.SendMessageAsync(request.);
                return Ok(new { Response = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
