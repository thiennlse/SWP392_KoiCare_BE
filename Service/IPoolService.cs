using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IPoolService
    {
        Task<List<PoolResponseModel>> GetAllPool();

        Task<Pool> GetPoolById(int id);

        Task AddNewPool(Pool pool);

        Task DeletePool(int id);

        Task<Pool> UpdatePool(Pool pool);
    }
}
