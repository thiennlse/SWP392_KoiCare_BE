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
        Task<List<BlogResponseModel>> GetAllBlog();

        Task<BlogResponseModel> AddNewBlog(Blog blog);

        Task DeleteBlog(int id);

        Task<BlogResponseModel> UpdateBlog(Blog blog);
    }
}
