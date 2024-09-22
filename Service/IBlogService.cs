using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    interface IBlogService
    {
        public Task<List<Blog>> GetAllBlog();

        public Task<Blog> GetBLogById(int id);

        public Task AddNewBlog(Blog blog);

        public Task DeleteBlog(int id);

        public Task<Blog> UpdateBlog(Blog blog);
    }
}
