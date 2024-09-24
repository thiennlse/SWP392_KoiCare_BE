using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IWaterService
    {
        Task<List<Waters>> GetAll();

        Task<Waters> GetById(int id);

        Task addWater(Waters water);

        Task<Waters> updateWater(Waters water);

        Task deleteWater(int id);
    }
}
