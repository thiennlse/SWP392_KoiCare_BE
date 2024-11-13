using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class FishService : IFishService
    {

        private IFishRepository _fishRepository;

        public FishService(IFishRepository fishRepository)
        {
            _fishRepository = fishRepository;
        }

        public async Task<List<Fish>> GetAllFishAsync(int page, int pageSize, string? searchTerm)
        {
            return await _fishRepository.GetAllFishAsync(page, pageSize, searchTerm);
        }

        public async Task<Fish> GetFishById(int id)
        {
            return await _fishRepository.GetById(id);
        }

        public async Task AddNewFish(Fish fish)
        {
            await _fishRepository.AddNewFish(fish);
        }

        public async Task DeleteFish(int id)
        {
            await _fishRepository.DeleteFish(id);
        }
        public async Task<Fish> UpdateById(Fish fish)
        {
            return await _fishRepository.UpdateById(fish);
        }
    }
}
