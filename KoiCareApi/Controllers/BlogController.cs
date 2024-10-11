﻿using BusinessObject.Models;
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
        public async Task<IActionResult> AddNewBlog([FromBody] BlogRequestModel _blog)
        {
            try
            {
                ValidationResult validationResult = _validation.Validate(_blog);
                if (!validationResult.IsValid)
                {
                    var error = validationResult.Errors.ToString();
                    return BadRequest(error);
                }
                Blog blog = new Blog
                {
                    MemberId = _blog.MemberId,
                    Title = _blog.Title,
                    Content = _blog.Content,
                    DateOfPublish = _blog.DateOfPublish,
                    Status = _blog.Status,
                    Member = await _memberService.GetMemberById(_blog.MemberId),
                };

                await _blogService.AddNewBlog(blog);

                return Ok(blog);
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
            var blog = await _blogService.GetBLogById(id);
            if (_blog == null)
            {
                return NotFound("blog no exits");
            }
            blog.Id = id;
            blog.MemberId = _blog.MemberId;
            blog.Title = _blog.Title;
            blog.Content = _blog.Content;
            blog.DateOfPublish = _blog.DateOfPublish;
            blog.Status = _blog.Status;
            blog.Member = await _memberService.GetMemberById(blog.MemberId);

            await _blogService.UpdateBlog(blog);

            return Ok(blog);
        }
    }
}
