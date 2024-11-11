using BusinessObject.Models;
using System;
using BusinessObject.ResponseModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IBlogRepository : IBaseRepository<Blog>
    {
        public  Task<List<Blog>> GetAllBlogAsync(int page, int pageSize, String? searchTerm);

        Task<BlogResponseModel> AddNewBlog(Blog blog);

        Task DeleteBlog(int id);

        Task<BlogResponseModel> UpdateBlog(Blog blog);

        public  Task<List<Blog>> GetAllPublishBlog();
    }
}
