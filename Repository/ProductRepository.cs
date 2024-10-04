using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly KoiCareDBContext _context;

        public ProductRepository(KoiCareDBContext context)
        {
            _context = context;
        }

        List<Product> ProductList;

        public async Task<List<Product>> GetAllProduct()
        {
            return await _context.Products.Include(b => b.User).ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.Include(b => b.User).SingleOrDefaultAsync(m => m.Id.Equals(id));
        }

        public async Task AddNewProduct(Product product)
        {
            if (product != null)
            {
                ProductList.Add(product);
                await _context.SaveChangesAsync();
            }

        }

        public async Task DeleteProduct(int id)
        {
            var product = await GetProductById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var _product = await GetProductById(product.Id);
            if (_product != null)
            {

            }
            await _context.SaveChangesAsync();
            return _product;
        }


    }
}
