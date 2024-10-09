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
    public class OrderRepository : IOrderRepository
    {
        private readonly KoiCareDBContext _context;

        public OrderRepository(KoiCareDBContext context)
        {
            _context = context;
        }

        List<Order> OrderList;

        public async Task<List<Order>> GetAllOrder()
        {
            return await _context.Orders.Include(b => b.Member).ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.Include(b => b.Member)
                .SingleOrDefaultAsync(m => m.Id.Equals(id));
        }

        public async Task AddNewOrder(Order order)
        {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(int id)
        {
            var order = await GetOrderById(id);
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


    }
}
