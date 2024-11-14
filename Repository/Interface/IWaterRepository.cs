using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IWaterRepository : IBaseRepository<Waters>
    {
        public Task<List<Waters>> GetAllWaterAsync(int page, int pageSize);

        Task addWater(Waters water);

        Task<Waters> updateWater(Waters water);

        Task deleteWater(int id);

        Task<Waters> GetWaterByIdProperties(int waterId);
    }
}
