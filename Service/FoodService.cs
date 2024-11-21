using BusinessObject.Models;
using BusinessObject.ResponseModel;
using iText.Layout.Properties;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
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

        public async Task<List<Food>> GetAllFoodAsync(int page, int pageSize, string? searchTerm)
        {
        return await _foodRepository.GetAllFoodAsync(page, pageSize, searchTerm);
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

        public async Task<double> CalculateDailyFishFood(int fishId)
        {
            double dailyFood = 0;
            double fishFoodResult = 0;
            Fish _fish = await _fishRepository.GetById(fishId);
            if (_fish != null)
            {
                var age = DateTime.Now.Year - _fish.Dob.Year;

                if (age > 0)
                {
                    if (age >= 25 && age <= 50)
                    {
                        dailyFood = (_fish.Weight * 1.5) / 100;
                        fishFoodResult = dailyFood * 0.3;
                    }
                    else if (age < 25)
                    {
                        fishFoodResult = (_fish.Weight * 0.5) / 100;
                    }
                }
                else
                {
                    int monthAge = DateTime.Now.Month - _fish.Dob.Month;
                    if (monthAge >= 1 && monthAge <= 4)
                    {
                        dailyFood = (_fish.Weight * 5) / 100;
                        fishFoodResult = dailyFood;
                    }
                    else if (monthAge >= 5 && monthAge <= 9)
                    {
                        dailyFood = (_fish.Weight * 4) / 100;
                        fishFoodResult = dailyFood;
                    }
                    else if (monthAge >= 10 && monthAge <= 12)
                    {
                        dailyFood = (_fish.Weight * 3) / 100;
                        fishFoodResult = dailyFood;
                    }
                    else if (monthAge == 0)
                    {
                        fishFoodResult = 0;
                    }
                }
            }

            return Math.Round(fishFoodResult, 2);
        }

        public async Task<double> CalculateWeeklyFoodRequirement(double dailyFood, Fish _fish) {
            int numberOfFeedDay = await GetFeedDay(_fish);
            // calculate weekly food 
            double weeklyFood = dailyFood * numberOfFeedDay;
            return Math.Round(weeklyFood, 2);
        }

        public async Task<int> GetFeedDay(Fish _fish)
        {
            int feedDay = 0;
            var ageYear = DateTime.Now.Year - _fish.Dob.Year;

            if (ageYear >= 1 && ageYear <= 10)
            {
                feedDay = 5;
            }
            else if (ageYear > 10)
            {
                feedDay = 3;
            }
            else
            {
                int monthAge = DateTime.Now.Month - _fish.Dob.Month;
                if (monthAge >= 1 && monthAge < 6)
                {
                    feedDay = 7;
                }
                else if (monthAge >= 6 && monthAge <= 12)
                {
                    feedDay = 6;
                }
            }

            return feedDay;
        }

        public async Task<double> CalculateFoodPerFeedingDay(double weeklyFood, Fish _fish)
        {
            int feedDay = await GetFeedDay(_fish);
            double foodPerFeedingDay = weeklyFood / feedDay;

            return Math.Round(foodPerFeedingDay, 2);
        }

    }
}
