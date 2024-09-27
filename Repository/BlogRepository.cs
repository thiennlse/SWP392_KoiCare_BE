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

namespace Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly KoiCareDBContext _context;

        public BlogRepository(KoiCareDBContext context)
        {
            _context = context;
        }

        List<Blog> blogList;

        public async Task<List<BlogResponseModel>> GetAllBlog()
        {
              List<Blog> blogs =  await _context.Blogs.Include(b => b.Member).ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            List<BlogResponseModel> _blogs = blogs.Select(b => mapper.Map<Blog, BlogResponseModel>(b)).ToList();

            return _blogs;

        }

        public async Task<Blog> GetBLogById(int id)
        {
            return await _context.Blogs.Include(b => b.Member).SingleOrDefaultAsync(m => m.Id.Equals(id));
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
          _context.Entry(blog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return blog;
        }


    }
}