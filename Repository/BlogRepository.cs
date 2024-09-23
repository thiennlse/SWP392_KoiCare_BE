using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly KoiCareDBContext _context;

        public BlogRepository(KoiCareDBContext context)
        {
            _context = context;
        }

        List<Blog> BlogList;

        public async Task<List<Blog>> GetAllBlog()
        {
            return await _context.Blogs.Include(b => b.MemberId).ToListAsync();
        }

        public async Task<Blog> GetBLogById(int id)
        {
            return await _context.Blogs.Include(b => b.MemberId).SingleOrDefaultAsync(m => m.Id.Equals(id));
        }

        public async Task AddNewBlog(Blog blog)
        {
            if (blog != null)
            {
               _context.Blogs.Add(blog); 
               await _context.SaveChangesAsync();
            }

        }

        public async Task DeleteBlog(int id)
        {
            var blog = await GetBLogById(id);
            if (blog != null) { 
            _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task<Blog> UpdateBlog(Blog blog)
        {
            var _blog = await GetBLogById( blog.Id);
            if (_blog != null) { 
                _blog.MemberId = blog.MemberId;
                _blog.Title = blog.Title;
                _blog.Content = blog.Content;
                _blog.DateOfPublish = blog.DateOfPublish;
                _blog.Status = blog.Status;
            }
            await _context.SaveChangesAsync();
            return _blog;
        }


    }
}