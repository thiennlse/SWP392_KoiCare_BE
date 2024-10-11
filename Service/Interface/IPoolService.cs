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
        public Task<List<Pool>> GetAllPool();

        public Task<Pool> GetPoolById(int id);

        public Task AddNewPool(Pool pool);

        public Task DeletePool(int id);

        public Task<Pool> UpdatePool(Pool pool);
    }
}
