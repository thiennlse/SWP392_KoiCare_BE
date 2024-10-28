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
                var email = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email);
                var name = claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name);
                var emailValue = email.Value;
                var nameValue = name.Value;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var jwtClaims = new[]
                {
                 email,
                 name
            };
                var Sectoken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
             _configuration["Jwt:Issuer"],
             claims: jwtClaims,
             expires: DateTime.Now.AddMinutes(60),
             signingCredentials: credentials);
                   
                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
                await _memberService.CreateMemberByGoogleAccount(emailValue, nameValue);
                return Ok(new { emailValue, nameValue, token });
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
                // Sign out locally
                await HttpContext.SignOutAsync();

                _logger.LogInformation("User logged out successfully");

                // Redirect to Google's logout endpoint to clear the Google session
                var googleLogoutUrl = "https://accounts.google.com/Logout";
                return Redirect(googleLogoutUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, "An error occurred while logging out");
            }
        }

        
    }
}
