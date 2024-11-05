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

        
    }
}
