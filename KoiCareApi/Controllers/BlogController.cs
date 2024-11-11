using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessObject.RequestModel;
using Microsoft.AspNetCore.Components.Web;
using Service.Interface;
using Validation.Blog;
using FluentValidation.Results;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IMemberService _memberService;
        private BlogValidation _validation;

        public BlogController(IBlogService blogService, IMemberService memberService, BlogValidation validation)
        {
            _blogService = blogService;
            _memberService = memberService;
            _validation = validation;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogAsync(int page = 1, int pageSize = 10, String? searchTerm = null)
        {
            var blog = await _blogService.GetAllBlogAsync(page, pageSize, searchTerm);
            if (blog == null)
            {
                return NotFound("No have any Blog");
            }
            return Ok(blog);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("please input id >0");
            }
            var blog = await _blogService.GetBLogById(id);
            if (blog == null)
            {
                return NotFound("no exits");
            }
            return Ok(blog);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewBlog([FromBody] BlogRequestModel _blog)
        {
            try
            {
                ValidationResult validationResult = _validation.Validate(_blog);
                if (validationResult.IsValid)
                {
                    var data = await _blogService.AddNewBlog(_blog);
                    return Ok(data);
                }
                var errors = validationResult.Errors.Select(e => (object)new
                {
                    e.PropertyName,
                    e.ErrorMessage
                }).ToList();
                return BadRequest(errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var _blog = await _blogService.GetBLogById(id);
            if (_blog == null)
            {
                return NotFound("blog no exits");
            }
            await _blogService.DeleteBlog(id);
            return NoContent();
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateBlog([FromBody] BlogRequestModel _blog, int id)
        {
            try
            {
                ValidationResult validationResult = _validation.Validate(_blog);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => (object)new
                    {
                        e.PropertyName,
                        e.ErrorMessage
                    }).ToList();
                    return BadRequest(errors);
                }
                var data = await _blogService.UpdateBlog(id, _blog);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("get-all-Publish-blog")]
        public async Task<IActionResult> GetAllPublishBlog()
        {
            var blog = await _blogService.GetAllPublishBlog();

            if (blog == null)
            {
                return BadRequest("no have Public Blog");
            }
            return Ok(blog);
        }
    }
}
