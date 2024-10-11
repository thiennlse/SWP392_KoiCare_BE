using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
using BusinessObject.IMapperConfig;
using Repository.Interface;

namespace Repository
{
    public class BlogRepository : BaseRepository<Blog>, IBlogRepository
    {
        private KoiCareDBContext _context;

        public BlogRepository(KoiCareDBContext context) : base(context)
        {
            _context = context;
        }

        List<Blog> blogList;

        public async Task<List<Blog>> GetAllBlog()
        {
            return await _context.Blogs
                .Include(b => b.Member)
                .ToListAsync();
        }

        public async Task<List<Blog>> GetAllBlogAsync(int page, int pageSize, String? searchTerm)
        {
            var query = GetQueryable();

            if (!string.IsNullOrEmpty(searchTerm)) {
                query = query.Where(p => p.Title.Contains(searchTerm) || p.Content.Contains(searchTerm));
            }
            var Blogs = await query.Skip((page - 1) * pageSize)
               .Take(pageSize).ToListAsync();
            return Blogs;
        }

        public async Task AddNewBlog(Blog blog)
        {
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBlog(int id)
        {
            var blog = await GetById(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<Blog> UpdateBlog(Blog blog)
        {
            _context.Entry(blog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return blog;
        }

    }
}