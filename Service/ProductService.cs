using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }



        public async Task<List<Product>> GetAllProduct(int page, int pageSize, string? searchTerm)
        {
            return await _productRepository.GetAllProductAsync(page, pageSize, searchTerm);
        }

        public async Task<Product> GetProductById(int id)
        {
            var result = await _productRepository.GetById(id);
            return result;
        }

        public async Task AddNewProduct(Product newProduct)
        {
            await _productRepository.UpdateProduct(newProduct);
        }

        public async Task DeleteProduct(int id)
        {
            _productRepository.DeleteProduct(id);
        }

        public async Task<Product> UpdateProduct(Product newProduct)
        {
            return await _productRepository.UpdateProduct(newProduct);
        }

        private ProductResponseModel MapToResponse(Product product)
        {
            return new ProductResponseModel
            {
                Id = product.Id,
                UserId = product.UserId,
                Name = product.Name,
                Image = product.Image,
                Cost = product.Cost,
                Description = product.Description,
                Origin = product.Origin,
                Productivity = product.Productivity,
                Code = product.Code,
                InStock = product.InStock
            };
        }
    }
}
