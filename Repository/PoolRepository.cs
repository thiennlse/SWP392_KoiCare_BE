using AutoMapper;
using BusinessObject.IMapperConfig;
using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PoolRepository : IPoolRepository
    {
        private readonly KoiCareDBContext _context;

        public PoolRepository(KoiCareDBContext context)
        {
            _context = context;
        }

        List<Pool> PoolList;

        public async Task<List<PoolResponseModel>> GetAllPool()
        {
          List<Pool> pool =  await _context.Pools.AsNoTracking().ToListAsync();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            List<PoolResponseModel> _pool = pool.Select(p => mapper.Map<Pool,PoolResponseModel>(p)).ToList();
            return _pool;

        }

        public async Task<Pool> GetPoolById(int id)
        {
            return await _context.Pools.Include(p => p.Water).SingleOrDefaultAsync(p => p.Id.Equals(id)); 
        }

        public async Task AddNewPool(Pool pool)
        {
                _context.Pools.Add(pool);
                await _context.SaveChangesAsync();
        }

        public async Task DeletePool(int id)
        {
            var pool = await GetPoolById(id);
            if (pool != null)
            {
                _context.Pools.Remove(pool);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<Pool> UpdatePool(Pool pool)
        {
            _context.Entry(pool).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return pool;
        }


    }
}
