using BusinessObject.Models;
using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMemberRepository _memberRepository;
        public BlogService(IBlogRepository blogRepository, IMemberRepository memberRepository)
        {
            _blogRepository = blogRepository;
            _memberRepository = memberRepository;
        }

        public async Task<List<Blog>> GetAllBlogAsync(int page, int pageSize, String? searchTerm)
        {
            return await _blogRepository.GetAllBlogAsync(page, pageSize, searchTerm);
        }

        public async Task<Blog> GetBLogById(int id)
        {
            return await _blogRepository.GetById(id);
        }

        public async Task DeleteBlog(int id)
        {
            await _blogRepository.DeleteBlog(id);
        }

        private Blog MapToDto(BlogRequestModel blog)
        {
            return new Blog
            {
                Image = blog.Image,
                Title = blog.Title,
                Content = blog.Content,
                DateOfPublish = blog.DateOfPublish.ToUniversalTime(),
                Status = blog.Status,
                MemberId = blog.MemberId
            };
        }

        public async Task<BlogResponseModel> AddNewBlog(BlogRequestModel blog)
        {
            var _blog = MapToDto(blog);
            return await _blogRepository.AddNewBlog(_blog);
        }

        public async Task<BlogResponseModel> UpdateBlog(int id, BlogRequestModel blog)
        {
            var _blog = MapToDto(blog);
            _blog.Id = id;
            return await _blogRepository.UpdateBlog(_blog);
        }

        public async Task<List<Blog>> GetAllPublishBlog()
        {
            return await _blogRepository.GetAllPublishBlog();
        }
    }
}
