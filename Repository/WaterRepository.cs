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

        public async Task<List<WaterResponseModel>> GetAll()
        {
            List<Waters> waters = await _context.Waters.AsNoTracking().ToListAsync();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            List<WaterResponseModel> _waters = waters.Select(w => mapper.Map<Waters, WaterResponseModel>(w)).ToList();

            return _waters;
        }

        public async Task<Waters> updateWater(Waters water)
        {
            _context.Entry(water).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return water;
        }
    }
}
