using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    interface IFoodService
    {
        public Task<List<Food>> GetAllFood();
        public Task<Food> GetFoodById(int id);
        public Task AddNewFood(Food food);
        public Task DeleteFoodById(int id);
        public Task<Food> UpdateFoodById(Food food);
    }
}
