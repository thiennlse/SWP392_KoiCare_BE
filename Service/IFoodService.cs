﻿using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
     public interface IFoodService
    {
        public Task<List<FoodResponseModel>> GetAllFood();
        public Task<Food> GetFoodById(int id);
        public Task AddNewFood(Food food);
        public Task DeleteFood(int id);
        public Task<Food> UpdateFood(Food food);
    }
}