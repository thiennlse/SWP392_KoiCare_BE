using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using BusinessObject.RequestModel;
namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMember()
        {
            var members = await _memberService.GetAllMember();
            if (members == null)
            {
                return NotFound("Không có member nào hiện tại");
            }
            return Ok(members);
        }
        [HttpGet("/{id}")]
        public async Task<IActionResult> GetMemeberById(int id)
        {
            if (id < 0)
            {
                return BadRequest("Vui lòng nhập đúng id");
            }
            var member = await _memberService.GetMemberById(id);
            if (member == null)
            {
                return NotFound($"Không có member với id {id}");
            }
            return Ok(member);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest("Vui lòng nhập đúng thông tin");
            }
            
            var member = await _memberService.Login(loginModel.Email, loginModel.Password);
            if (member == null)
            {
                return Unauthorized("Sai tài khoản hoặc mật khẩu");
            }
            
            return Ok(member);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Member member)
        {
            if (member == null)
            {
                return BadRequest(new { message = "Vui lòng nhập đúng thông tin" });
            }

            if (string.IsNullOrWhiteSpace(member.Email) || string.IsNullOrWhiteSpace(member.Password))
            {
                return BadRequest(new { message = "Email và mật khẩu không được để trống" });
            }

            if (await _memberService.ExistedEmail(member.Email))
            {
                return BadRequest(new { message = "Email đã được sử dụng" });
            }
            await _memberService.Register(member);
            return Created("Created",member);
        }
    }
}
