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
        Task<List<Product>> GetAllProduct(int page, int pageSize, string? searchTerm);

        Task<Product> GetProductById(int id);

        Task AddNewProduct(Product product);

        Task DeleteProduct(int id);

        Task<Product> UpdateProduct(Product product);
    }
}
