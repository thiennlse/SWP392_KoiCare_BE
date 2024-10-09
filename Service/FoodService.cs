using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Repository;
using Repository.Interface;
using Service.Interface;
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

        public async Task<List<FoodResponseModel>> GetAllFood() 
        {
        return await _foodRepository.GetAllFood();
        }

        public async Task<Food> GetFoodById(int id) 
        {
        return await _foodRepository.GetById(id);
        }

        public async Task AddNewFood(Food food) 
        {
         await _foodRepository.AddNewFood(food);
        }

        public async Task DeleteFood(int id) 
        {
            await _foodRepository.DeleteFood(id);
        }

        public async Task<Food> UpdateFood(Food food) 
        {
         return await _foodRepository.UpdateFood(food);
        }
    }
}
