using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FoodRepository : IFoodRepository
    {
        private readonly KoiCareDBContext _context;

        public FoodRepository(KoiCareDBContext context)
        {

            _context = context;
        }

        List<Food> FoodList;

        public async Task<List<Food>> GetAllFood()
        {
            return await _context.Foods.ToListAsync();
        }

        public async Task<Food> GetFoodById(int id)
        {
            return await _context.Foods.SingleOrDefaultAsync(f => f.Id.Equals(id));
        }

        public async Task AddNewFood(Food food)
        {
            if (food != null)
            {
                FoodList.Add(food);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteFoodById(int id)
        {
            var food = await GetFoodById(id);
            if (food != null)
            {
                _context.Foods.Remove(food);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Food> UpdateFoodById(Food food) 
        {
            var _food = await GetFoodById(food.Id);

            if(_food != null) 
            {
              _food.Name = food.Name;
              _food.Weight = food.Weight;
            _food.Fish = food.Fish;
            }
            await _context.SaveChangesAsync();
            return _food;
        }
    }
}
