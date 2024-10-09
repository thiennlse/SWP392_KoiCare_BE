using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IWaterRepository
    {
        Task<List<WaterResponseModel>> GetAll();

        Task<Waters> GetById(int id);

        Task addWater(Waters water);

        Task<Waters> updateWater(Waters water);

        Task deleteWater(int id);
    }
}
