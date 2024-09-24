using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllProduct();

        public Task<Product> GetProductById(int id);

        public Task AddNewProduct(Product product);

        public Task DeleteProduct(int id);

        public Task<Product> UpdateProduct(Product product);
    }
}
