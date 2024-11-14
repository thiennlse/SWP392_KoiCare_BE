using BusinessObject.Models;
using BusinessObject.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProduct(int page, int pageSize, string? searchTerm);

        Task<Product> GetProductById(int id);

        Task AddNewProduct(ProductRequestModel product);

        Task DeleteProduct(int id);

        Task<Product> UpdateProduct(int id, ProductRequestModel product);

        public  Task<List<Product>> GetListProductbyListProductid(List<int> listProductId);
    }
}
