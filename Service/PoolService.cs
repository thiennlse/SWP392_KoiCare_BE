using BusinessObject.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PoolService : IPoolService
    {
        private readonly IPoolRepository _poolRepository;
        public PoolService(IPoolRepository poolRepository)
        {
            _poolRepository = poolRepository;
        }



        public async Task<List<Pool>> GetAllPool()
        {
            return await _poolRepository.GetAllPool();
        }

        public async Task<Pool> GetPoolById(int id)
        {
            return await _poolRepository.GetPoolById(id);
        }

        public async Task AddNewPool(Pool newPool)
        {
            await _poolRepository.UpdatePool(newPool);
        }

        public async Task DeletePool(int id)
        {
            _poolRepository.DeletePool(id);
        }

        public async Task<Pool> UpdatePool(Pool newPool)
        {
            return await _poolRepository.UpdatePool(newPool);
        }
    }
}
