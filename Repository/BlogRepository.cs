using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.ResponseModel;
using AutoMapper;
using BusinessObject.IMapperConfig;
using Repository.Interface;
using System.Reflection.Metadata;

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

        public async Task<BlogResponseModel> AddNewBlog(Blog blog)
        {
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            return MapToResponse(blog);
        }

        public async Task<BlogResponseModel> UpdateBlog(Blog blog)
        {
            _context.Entry(blog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return MapToResponse(blog);
        }
        public async Task DeleteBlog(int id)
        {
            _dbSet.Remove(await GetById(id));
            await _context.SaveChangesAsync();
        }
        private BlogResponseModel MapToResponse(Blog blog)
        {
            return new BlogResponseModel
            {
                Title = blog.Title,
                Content = blog.Content,
                DateOfPublish = blog.DateOfPublish,
                Status = blog.Status,
                Member = _context.Members.FirstOrDefault(b => b.Id == blog.MemberId)
            };
        }

        public  async Task<List<Blog>> GetAllPublishBlog()
        {
            List<Blog> listBlogPrivate = await _context.Blogs
                .Where(blog => blog.Status.Equals("Publish")).AsNoTracking()
                .ToListAsync();

            return listBlogPrivate;
        }
    }
}