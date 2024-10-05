using AutoMapper;
using BusinessObject.IMapperConfig;
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
    public class ProductRepository : IProductRepository
    {
        private readonly KoiCareDBContext _context;

        public ProductRepository(KoiCareDBContext context)
        {
            _context = context;
        }

        List<Product> ProductList;

        public async Task<List<ProductResponseModel>> GetAllProduct()
        {
            List<Product> products = await _context.Products.AsNoTracking().ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            List<ProductResponseModel> _products = products.Select(p =>  mapper.Map<Product,ProductResponseModel>(p)).ToList();

            return _products;
        }
        
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.Include(b => b.User)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id.Equals(id));
        }

        public async Task AddNewProduct(Product product)
        {
          
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            

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
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }


    }
}
