using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IRoleService _roleService;

        public MemberController(IMemberService memberService, IRoleService roleService)
        {
            _memberService = memberService;
            _roleService = roleService;
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
            MemberResponseModel response = new MemberResponseModel();
            response.Id = member.Id;
            response.Email = member.Email;
            response.Password = member.Password;
            response.Phone = member.Phone;
            response.FullName = member.FullName;
            response.Address = member.Address;
            response.Role = member.Role;
            if (member == null)
            {
                return NotFound($"Không có member với id {id}");
            }
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountRequestModel loginModel)
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
        public async Task<IActionResult> Register([FromBody] AccountRequestModel member)
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
            Member _member = new Member();
            _member.Email = member.Email;
            _member.Password = member.Password;
            _member.RoleId = 4;
            _member.Role = await _roleService.GetRoleById(_member.RoleId);
            await _memberService.Register(_member);
            return Created("Created",_member);
        }
    }
}
