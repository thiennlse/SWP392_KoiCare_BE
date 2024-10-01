using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class WaterRepository : IWaterRepository
    {
        private readonly KoiCareDBContext _context;
        public WaterRepository(KoiCareDBContext context) 
        {
            _context = context;
        }


        public async Task addWater(Waters water)
        {
            _context.Waters.Add(water);
            await _context.SaveChangesAsync();
        }

        public async Task deleteWater(int id)
        {
            Waters waters =  await GetById(id);
            _context.Waters.Add(waters);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Waters>> GetAll()
        {
            return await _context.Waters.ToListAsync();
        }

        public async Task<Waters> GetById(int id)
        {
            return await _context.Waters.SingleOrDefaultAsync(w => w.Id.Equals(id));
        }

        public async Task<Waters> updateWater(Waters water)
        {
            _context.Entry(water).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return water;
        }
    }
}
