using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IProductRepository
    {
        public Task<List<ProductResponseModel>> GetAllProduct();

        public Task<Product> GetProductById(int id);

        public Task AddNewProduct(Product product);

        public Task DeleteProduct(int id);

        public Task<Product> UpdateProduct(Product product);
    }
}
