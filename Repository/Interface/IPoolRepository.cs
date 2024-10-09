using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IPoolRepository : IBaseRepository<Pool>
    {
        Task<List<Pool>> GetAllPool();

        Task AddNewPool(Pool pool);

        Task DeletePool(int id);

        Task<Pool> UpdatePool(Pool pool);
    }
}
