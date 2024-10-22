using AutoMapper;
using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using BusinessObject.IMapperConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class WaterRepository : BaseRepository<Waters>, IWaterRepository
    {
        private readonly KoiCareDBContext _context;
        public WaterRepository(KoiCareDBContext context) :base(context)
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
            _context.Waters.Remove(waters);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Waters>> GetAllWaterAsync(int page, int pageSize)
        {
            var query = GetQueryable();
            
            var products = await query.Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();
            return products.ToList();
        }

        public async Task<Waters> updateWater(Waters water)
        {
            _context.Entry(water).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return water;
        }

        
    }
}
