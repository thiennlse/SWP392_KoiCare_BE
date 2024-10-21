using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public Task<List<Product>> GetAllProduct();

        Task<List<Product>> GetAllProductAsync(int page, int pageSize, string? searchTerm);
        public Task AddNewProduct(Product product);
        public Task DeleteProduct(int id);
        public Task<Product> UpdateProduct(Product product);
        Product getById(int id);
    }
}
