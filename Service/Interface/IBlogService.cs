using BusinessObject.Models;
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

        Task AddNewBlog(Blog blog);

        Task DeleteBlog(int id);

        Task<Blog> UpdateBlog(Blog blog);
    }
}
