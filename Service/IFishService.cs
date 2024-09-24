using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IFishService
    {
        public Task<List<Fish>> GetAllFish();
        public Task<Fish> GetFishById(int id);
        public Task AddNewFish(Fish fish);
        public Task DeleteById(int id);
        public Task<Fish> UpdateById(Fish fish);
    }
}
