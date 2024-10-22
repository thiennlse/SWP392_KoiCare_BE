using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using Service.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
namespace KoiCareApi.Controllers
{
    [Route("api/Member")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IRoleService _roleService;
        private IConfiguration _config;

        public MemberController(IMemberService memberService, IRoleService roleService, IConfiguration config)
        {
            _memberService = memberService;
            _roleService = roleService;
            _config = config;
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
        [HttpGet("{id}")]
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

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                new Claim(ClaimTypes.Role, member.Role.Name),
            };

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims: claim,
              expires: DateTime.Now.AddMinutes(60),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
            AccountResponse response = new AccountResponse
            {
                Success = true,
                Token = token,
                Role = member.Role.Name,
                UserId = member.Id
            };
            return Ok(response);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRequestModel _registerMember)
        {

            if (_registerMember == null)
            {
                return BadRequest(new { message = "Vui lòng nhập đúng thông tin" });
            }

            if (string.IsNullOrWhiteSpace(_registerMember.Email) || string.IsNullOrWhiteSpace(_registerMember.Password))
            {
                return BadRequest(new { message = "Email và mật khẩu không được để trống" });
            }

            if (await _memberService.ExistedEmail(_registerMember.Email))
            {
                return BadRequest(new { message = "Email đã được sử dụng" });
            }
            Member _member = new Member();
            _member.Email = _registerMember.Email;
            _member.Password = _registerMember.Password;
            _member.RoleId = 4;
            _member.Role = await _roleService.GetRoleById(_member.RoleId);
            await _memberService.Register(_member);
            return Created("Created", _member);
        }
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update([FromBody] MemberRequestModel model, int id)
        {
            if ( await _memberService.GetMemberById(id) != null)
            {
                if (model != null)
                {
                    Member member = await _memberService.GetMemberById(id);
                    member.Email = model.Email;
                    member.Password = model.Password;
                    member.Image = model.Image;
                    member.Phone = model.Phone;
                    member.FullName = model.FullName;
                    member.Address = model.Address;
                    member.RoleId = model.RoleId;
                    await _memberService.UpdateMember(member);
                    return Ok(member);
                }
                return BadRequest("Error with input");
            }
            return NotFound();
        }
    }
}
