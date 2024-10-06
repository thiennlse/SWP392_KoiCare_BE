using BusinessObject.Models;
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



        public async Task<List<Product>> GetAllProduct()
        {
            return await _productRepository.GetAllProduct();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
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
    }
}
