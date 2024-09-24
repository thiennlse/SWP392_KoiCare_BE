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

        private List<Fish> fishtList;

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
            _context.Entry(fish).State = EntityState.Modified;
              await _context.SaveChangesAsync();
            return fish;
        }


    }
}
