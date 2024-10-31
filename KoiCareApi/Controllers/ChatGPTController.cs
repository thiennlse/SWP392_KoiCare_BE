using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace KoiCareApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatGPTController : ControllerBase
    {
        private readonly ChatGPTService _chatGptClient;

        public ChatGPTController(ChatGPTService chatGptClient)
        {
            _chatGptClient = chatGptClient;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatGPTRequestModel request)
        {
            try
            {
                var responseMessage = await _chatGptClient.SendMessageAsync(request);
                var response = new ChatGPTResponseModel
                {
                    Message = responseMessage,
                    Success = true
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ChatGPTResponseModel
                {
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
