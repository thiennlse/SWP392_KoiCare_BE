using BusinessObject.Models;
using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IBlogService
    {
        public  Task<List<Blog>> GetAllBlogAsync(int page, int pageSize, String? searchTerm);

        Task<Blog> GetBLogById(int id);

        Task<BlogResponseModel> AddNewBlog(BlogRequestModel blog);

        Task DeleteBlog(int id);

        Task<BlogResponseModel> UpdateBlog(int id,BlogRequestModel blog);

        public Task<List<Blog>> GetAllPublishBlog();
    }
}
