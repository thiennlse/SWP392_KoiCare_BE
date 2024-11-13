using BusinessObject.Models;
using BusinessObject.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ISubcriptionService
    {
        Task<List<Subcriptions>> GetAll();

        Task<Subcriptions> GetById(int id);

        Task Add(SubcriptionRequest request);

        Task Update(int id, SubcriptionRequest request);

        Task DeleteById(int id);
    }
}
