using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IFishRepository : IBaseRepository<Fish>
    {
        public  Task<List<FishResponseModel> GetAllFish();
        public  Task AddNewFish(Fish fish);
        public  Task DeleteFish(int id);
        public  Task<Fish> UpdateById(Fish fish);
    }
}
