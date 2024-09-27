﻿using BusinessObject.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        public BlogService( IBlogRepository blogRepository) { 
            _blogRepository = blogRepository;
        }

        public async Task<List<Blog>> GetAllBlog()
        {
               return await _blogRepository.GetAllBlog();
        }

        public async Task<Blog> GetBLogById(int id)
        {
            return await _blogRepository.GetBLogById(id);
        }

        public async Task AddNewBlog(Blog newBlog) { 
         await _blogRepository.UpdateBlog(newBlog);
        }
      
        public async Task DeleteBlog(int id)
        {
              _blogRepository.DeleteBlog(id);
        }

        public  async Task<Blog> UpdateBlog(Blog newBlog)
        {
          return  await _blogRepository.UpdateBlog(newBlog);
        }
    }
}
