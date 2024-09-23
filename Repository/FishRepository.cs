using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FishRepository : IFishRepository
    {
        private readonly KoiCareDBContext _context;

        public FishRepository(KoiCareDBContext context)
        {
            _context = context;

        }

        private List<Fish> FistList;

        public async Task<List<Fish>> GetAllFish() 
        { 
        return await _context.Fishes.ToListAsync();
        }

      public async Task<Fish> GetFishById(int id) 
        {
            return await _context.Fishes.SingleOrDefaultAsync(f => f.Id.Equals(id));
        }

      public async Task AddNewFish(Fish fish)
        {
            if (fish != null) {
              _context.Fishes.AddAsync(fish);   
                await _context.SaveChangesAsync();
            }

        }

        public async Task DeleteById(int id)
        {
            var _fish = await GetFishById(id);
            if ( _fish != null) {
                _context.Fishes.Remove( _fish );
                await _context.SaveChangesAsync();
            }

        }

        public  async Task<Fish> UpdateById(Fish fish)
        {
            var _fish = await GetFishById(fish.Id);

            if (_fish != null) 
            { 
            _fish.Weight = fish.Weight;
                _fish.Origin = fish.Origin;
                _fish.Size = fish.Size;
                _fish.FoodId = fish.FoodId;
                _fish.PoolId = fish.PoolId;
                _fish.Age = fish.Age;
                _fish.Gender = fish.Gender;
                _fish.Image = fish.Image;
                _fish.Name = fish.Name;
                await _context.SaveChangesAsync();
            }
            return _fish;
        }


    }
}
