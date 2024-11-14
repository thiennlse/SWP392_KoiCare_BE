using BusinessObject.Models;
using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IPoolService
    {
        public Task<List<Pool>> GetAllPoolAsync(int page, int pageSize, String? searchTerm);

        Task<Pool> GetPoolById(int id);

        Task AddNewPool(PoolRequestModel pool);

        Task DeletePool(int id);

        Task UpdatePool(int id,PoolRequestModel request);

        public  Task<WaterElementResponseModel> CheckWaterElementInPool(int PoolId);
        public  Task<int> TotalFishInPool(int poolId);
        public Task<List<Fish>> GetFishByPoolId(int poolId);
    }
}
