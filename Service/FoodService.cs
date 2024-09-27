using BusinessObject.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;

        public FoodService(IFoodRepository foodRepository) 
        {
        _foodRepository = foodRepository;
        }

        public async Task<List<Food>> GetAllFood() 
        {
        return await _foodRepository.GetAllFood();
        }

        public async Task<Food> GetFoodById(int id) 
        {
        return await _foodRepository.GetFoodById(id);
        }

        public async Task AddNewFood(Food food) 
        {
         await _foodRepository.AddNewFood(food);
        }

        public async Task DeleteFoodById(int id) 
        {
            await _foodRepository.DeleteFoodById(id);
        }

        public async Task<Food> UpdateFoodById(Food food) 
        {
         return await _foodRepository.UpdateFoodById(food);
        }
    }
}
