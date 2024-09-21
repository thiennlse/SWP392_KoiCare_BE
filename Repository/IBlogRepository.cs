﻿using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IBlogRepository
    {
        public Task<List<Blog>> GetAllBlog();
        public Task<Blog> GetBLogById(int id);
        public void AddNewBlog(Blog newBlog);
        public  void DeleteBlog(int id);

        public Task<Blog> UpdateBlog(Blog newBlog);
    }
}
