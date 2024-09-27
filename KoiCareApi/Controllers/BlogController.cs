using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlog()
        {
            var blog = await _blogService.GetAllBlog();
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
        public async Task<IActionResult> AddNewBlog([FromBody] Blog _blog)
        {
            if (_blog == null) {
                return BadRequest("please input Blog information");
            }
            await _blogService.AddNewBlog(_blog);
            return Ok("add successfully");
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var _blog = await _blogService.GetBLogById(id);
            if (_blog == null) {
                return NotFound("blog no exits");
            }
            await _blogService.DeleteBlog(id);
            return Ok("delete successfully");
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateById([FromBody]Blog _blog) 
        {
            var blog = await _blogService.GetBLogById(_blog.Id);
            if (blog == null)
            {
                return NotFound("blog no exits");
            }
            await _blogService.UpdateBlog(_blog);
            return Ok("update successfully");
        }
    }
}
