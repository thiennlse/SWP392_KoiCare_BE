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
        public Task<Fish> GetFishById(int id);
        public Task AddNewFish(Fish fish);
        public Task DeleteFish(int id);
        public Task<Fish> UpdateById(Fish fish);
    }
}
