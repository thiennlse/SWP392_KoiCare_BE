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
            return await _context.Blogs.ToListAsync();
        }

        public async Task<Blog> GetBLogById(int id)
        {
            return await _context.Blogs.SingleOrDefaultAsync(m => m.Id.Equals(id));
        }

        public async void AddNewBlog(Blog newBlog)
        {
            if (newBlog != null)
            {
                BlogList.Add(newBlog);
                await _context.SaveChangesAsync();
            }

        }

        public async void DeleteBlog(int id)
        {
            var Blog = await _context.Blogs.SingleOrDefaultAsync(b => b.Id == id);
            if (Blog != null) { 
            _context.Blogs.Remove(Blog);
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task<Blog> UpdateBlog(Blog newBlog)
        {
            var Blog = await _context.Blogs.SingleOrDefaultAsync(b => b.Id == newBlog.Id);
            if (Blog != null) { 
                Blog.MemberId = newBlog.MemberId;
               Blog.Title = newBlog.Title;
                Blog.Content = newBlog.Content;
                Blog.DateOfPublish = newBlog.DateOfPublish;
                Blog.Status = newBlog.Status;
            }
            await _context.SaveChangesAsync();
            return Blog;
        }


    }
}