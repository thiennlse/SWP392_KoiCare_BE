using BusinessObject.RequestModel;
using Microsoft.AspNetCore.Http;
using Repository.Interface;
using Service.Interface;
using System.Security.Claims;
namespace Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public ProductService(IProductRepository productRepository, IHttpContextAccessor contextAccessor)
        {
            _productRepository = productRepository;
            _contextAccessor = contextAccessor;
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

        public async Task AddNewProduct(ProductRequestModel newProduct)
        {
            Product product = MapToProduct(newProduct);
            await _productRepository.AddNewProduct(product);
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
        }

        public async Task<Product> UpdateProduct(int id, ProductRequestModel newProduct)
        {
            Product product = MapToProduct(newProduct);
            product.Id = id;
            return await _productRepository.UpdateProduct(product);
        }

        private Product MapToProduct(ProductRequestModel product)
        {
            var currUser = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currUserId = int.Parse(currUser);
            return new Product
            {
                UserId = currUserId,
                Name = product.Name,
                Image = product.Image,
                Cost = product.Cost,
                Description = product.Description,
                Origin = product.Origin,
                Productivity = product.Productivity,
                Code = GenerateUniqueCode(),
                InStock = product.InStock
            };
        }
        private string GenerateUniqueCode() => Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
    }
}
