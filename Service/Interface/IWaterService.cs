using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IWaterService 
    {
        public  Task<List<Waters>> GetAllWaterAsync(int page, int pageSize);

        Task<Waters> GetById(int id);

        Task addWater(Waters water);

        Task<Waters> updateWater(Waters water);

        Task deleteWater(int id);
    }
}
