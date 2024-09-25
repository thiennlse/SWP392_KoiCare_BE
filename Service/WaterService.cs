using BusinessObject.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class WaterService : IWaterService
    {

        private readonly IWaterRepository _repo;

        public WaterService(IWaterRepository repo)
        {
            _repo = repo;
        }

        public async Task addWater(Waters water)
        {
            await _repo.addWater(water);
        }

        public async Task deleteWater(int id)
        {
           await _repo.deleteWater(id);
        }

        public async Task<List<Waters>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Waters> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task<Waters> updateWater(Waters water)
        {
            return await _repo.updateWater(water);
        }
    }
}
