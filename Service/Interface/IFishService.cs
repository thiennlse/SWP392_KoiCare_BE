using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IFishService
    {
        public  Task<List<Fish>> GetAllFishAsync(int page, int pageSize, string? searchTerm);
        public  Task<Fish> GetFishById(int fishId);
        public Task AddNewFish(Fish fish);
        public Task DeleteFish(int id);
        public Task<Fish> UpdateById(Fish fish);
        public  Task<Fish> GetFishByIdGetFishProperties(int fishId);
        public Task<FishProperties> GetFishPropertiesForCalculateByFishId(int fishId);
        public Task<Fish> GetLastPropertiesOnDay(int fishId);
    }
}
