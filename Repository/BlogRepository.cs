﻿using BusinessObject.Models;
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