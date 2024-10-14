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

        public async Task<List<BlogResponseModel>> GetAllBlog()
        {
            List<Blog> blogs = await _context.Blogs.Include(b => b.Member).ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            List<BlogResponseModel> _blogs = blogs.Select(b => mapper.Map<Blog, BlogResponseModel>(b)).ToList();

            return _blogs;

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
    }
}