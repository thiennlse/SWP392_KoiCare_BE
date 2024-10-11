using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.ResponseModel;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PoolRepository : BaseRepository<Pool>, IPoolRepository
    {
        private readonly KoiCareDBContext _context;

        public PoolRepository(KoiCareDBContext context) : base(context)
        {
            _context = context;
        }

        List<Pool> PoolList;

        public async Task<List<Pool>> GetAllPool()
        {
            return await _context.Pools.Include(b => b.Member).ToListAsync();
        }

       
        public async Task AddNewPool(Pool pool)
        {
            if (pool != null)
            {
                _context.Pools.Add(pool);
                await _context.SaveChangesAsync();
            }

        }

        public async Task DeletePool(int id)
        {
            var pool = await GetById(id);
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
