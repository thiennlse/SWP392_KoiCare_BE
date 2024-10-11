using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllProduct(int page, int pageSize, string? searchTerm);

        public Task<Product> GetProductById(int id);

        public Task AddNewProduct(Product product);

        public Task DeleteProduct(int id);

        public Task<Product> UpdateProduct(Product product);
    }
}
