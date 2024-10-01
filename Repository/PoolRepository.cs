using BusinessObject.Models;
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

        public async Task<List<Pool>> GetAllPool()
        {
            return await _context.Pools.Include(b => b.User).ToListAsync();
        }

        public async Task<Pool> GetPoolById(int id)
        {
            return await _context.Pools.Include(b => b.User).SingleOrDefaultAsync(m => m.Id.Equals(id));
        }

        public async Task AddNewPool(Pool pool)
        {
            if (pool != null)
            {
                PoolList.Add(pool);
                await _context.SaveChangesAsync();
            }

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
            var _pool = await GetPoolById(pool.Id);
            if (_pool != null)
            {

            }
            await _context.SaveChangesAsync();
            return _pool;
        }


    }
}
