using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper.Execution;
using Service.Interface;
using Org.BouncyCastle.Security;
using BusinessObject.Models;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;
        private readonly IMemberService _memberService;
        public AccountController(IConfiguration configuration, ILogger<AccountController> logger, IMemberService memberService)
        {
            _configuration = configuration;
            _logger = logger;
            _memberService = memberService;
        }

        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            try
            {
                var properties = new AuthenticationProperties
                {
                    RedirectUri = Url.Action(nameof(GoogleCallback)),
                    // Add additional parameters if needed
                    Items =
                {
                    { "LoginProvider", GoogleDefaults.AuthenticationScheme },
                    { "scheme", GoogleDefaults.AuthenticationScheme },
                }
                };

                return Challenge(properties, GoogleDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating Google authentication");
                return StatusCode(500, "An error occurred while initiating Google login");
            }
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

                if (!result.Succeeded)
                {
                    return BadRequest("Google authentication failed");
                }

                var claims = result.Principal.Claims;
                var emailClaim = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email);
                var nameClaim = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name);

                if (emailClaim == null || nameClaim == null)
                {
                    return BadRequest("Email or Name claim is missing");
                }

                var emailValue = emailClaim.Value;
                var nameValue = nameClaim.Value;

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var emailExisted = await _memberService.ExistedEmail(emailValue);

                if (emailExisted != null)
                {
                    if (emailExisted.Password == "1")
                    {
                        var jwtClaims = new[]
                        {
                    new Claim(ClaimTypes.NameIdentifier, emailExisted.Id.ToString()),
                };

                        var tokenDescriptor = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Issuer"],
                            claims: jwtClaims,
                            expires: DateTime.Now.AddMinutes(60),
                            signingCredentials: credentials
                        );

                        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

                        return Ok(new { emailValue, nameValue, token });
                    }

                    return BadRequest("Email has existed");
                }

                var data = await _memberService.CreateMemberByGoogleAccount(emailValue, nameValue);
                var newJwtClaims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, data.Id.ToString()),
                    new Claim(ClaimTypes.Role, data.Role.Name)
                };

                var newTokenDescriptor = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Issuer"],
                    claims: newJwtClaims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: credentials
                );

                var newToken = new JwtSecurityTokenHandler().WriteToken(newTokenDescriptor);

                return Ok(new { emailValue, nameValue, token = newToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Google callback");
                return StatusCode(500, "An error occurred while processing Google authentication");
            }
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                _logger.LogInformation("User logged out successfully");
                return Ok("User logged out successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return BadRequest("An error occurred while logging out");
            }
        }


    }
}
