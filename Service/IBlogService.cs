using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IBlogService
    {
        public Task<List<BlogResponseModel>> GetAllBlog();

        public Task<Blog> GetBLogById(int id);

        public Task AddNewBlog(Blog blog);

        public Task DeleteBlog(int id);

        public Task<Blog> UpdateBlog(Blog blog);
    }
}
