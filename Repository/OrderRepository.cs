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
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly KoiCareDBContext _context;

        public OrderRepository(KoiCareDBContext context) : base(context)
        {
            _context = context;
        }

        List<Order> OrderList;

        public async Task<List<Order>> GetAllOrderAsync(int page, int pageSize, string? searchTerm)
        {
            var query = GetQueryable()
                .Include(o => o.OrderProducts)
                .AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(o => o.Code.Contains(searchTerm) 
                || o.Description.Contains(searchTerm));
            }

            var orders = await query.Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();
            return orders.ToList();
        }

        public async Task AddNewOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(int id)
        {
            var order = await GetById(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return order;
        }

        public Order getById(int id)
        {
            return _context.Orders.FirstOrDefault(o => o.Id.Equals(id));
        }
    }
}
