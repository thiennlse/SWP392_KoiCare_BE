using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly KoiCareDBContext _context;

        public ProductRepository(KoiCareDBContext context) : base(context)
        {
            _context = context;
        }

        List<Product> ProductList;

        public async Task<List<Product>> GetAllProduct()
        {
            return await _context.Products.Include(b => b.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Product>> GetAllProductAsync(int page, int pageSize, string? searchTerm)
        {
            var query = GetQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) || p.Origin.Contains(searchTerm));
            }

            var products = await query.Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();
            return products.ToList();
        }

        public async Task AddNewProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await GetById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
