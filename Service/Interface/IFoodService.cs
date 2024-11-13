using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IFoodService
    {
        public  Task<List<Food>> GetAllFoodAsync(int page, int pageSize, string? searchTerm);
        public Task<Food> GetFoodById(int id);
        public Task AddNewFood(Food food);
        public Task DeleteFood(int id);
        public Task<Food> UpdateFood(Food food);
        public  Task<double> CalculateDailyFishFood(int fishId);
        public  Task<double> CalculateWeeklyFoodRequirement(double dailyFood, Fish _fish);
        public  Task<int> GetFeedDay(Fish _fish);
        public  Task<double> CalculateFoodPerFeedingDay(double weeklyFood, Fish _fish);
    }
}
