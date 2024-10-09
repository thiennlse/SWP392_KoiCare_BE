using AutoMapper;
using BusinessObject.IMapperConfig;
using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository
{
    public class FoodRepository : BaseRepository<Food> ,IFoodRepository
    {
        private readonly KoiCareDBContext _context;

        public FoodRepository(KoiCareDBContext context) : base(context)
        {

            _context = context;
        }

        List<Food> foodList;

        public async Task<List<FoodResponseModel>> GetAllFood()
        {
            List<Food> foods = await _context.Foods.ToListAsync();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            List<FoodResponseModel> _foods = foods.Select(f => mapper.Map<Food, FoodResponseModel>(f)).ToList();

            return _foods;
        }

        public async Task AddNewFood(Food food)
        {
                _context.Foods.Add(food);
                await _context.SaveChangesAsync();
        }

        public async Task DeleteFood(int id)
        {
            var food = await GetById(id);
            if (food != null)
            {
                _context.Foods.Remove(food);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Food> UpdateFood(Food food) 
        {
            _context.Entry(food).State = EntityState.Modified;
            await _context.SaveChangesAsync();
           return food;
        }
    }
}
