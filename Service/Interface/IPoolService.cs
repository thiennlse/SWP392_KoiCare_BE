using BusinessObject.Models;
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

        Task AddNewPool(Pool pool);

        Task DeletePool(int id);

        Task<Pool> UpdatePool(Pool pool);
        public Task<Double> CalCulateSaltPoolNeed(int poolId);
    }
}
