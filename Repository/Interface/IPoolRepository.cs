using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IPoolRepository : IBaseRepository<Pool>
    {
        public  Task<List<Pool>> GetAllPooltAsync(int page, int pageSize, String? searchTerm);

        Task AddNewPool(Pool pool);

        Task DeletePool(int id);

        Task<Pool> UpdatePool(Pool pool);
    }
}
