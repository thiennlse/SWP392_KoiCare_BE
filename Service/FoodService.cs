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
        private readonly IFishRepository _fishRepository;

        public FoodService(IFoodRepository foodRepository, IFishRepository fishRepository) 
        {
        _foodRepository = foodRepository;
            _fishRepository = fishRepository;
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

        public async Task<double> CalculateFishFood(int fishId)
        {
            double dailyFood =  0;
            double fishFoodResult = 0;
           
            Fish _fish = await _fishRepository.GetById(fishId);
            var age = DateTime.Now.Year - _fish.Dob.Year;
            if (_fish != null) {              
                if (age >= 25 && age <= 50)
                {
                    dailyFood = (_fish.Weight * 1.5) / 100;
                    fishFoodResult = dailyFood * 0.3;
                }
                if(age < 25) 
                {
                    fishFoodResult = (_fish.Weight * 0.5) / 100;
                }
            }

            return Math.Round(fishFoodResult, 2);
             

        }

        
    }
}
