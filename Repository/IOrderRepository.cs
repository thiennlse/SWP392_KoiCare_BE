using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IOrderRepository
    {
        public Task<List<Order>> GetAllOrder();

        public Task<Order> GetOrderById(int id);

        public Task AddNewOrder(Order order);

        public Task DeleteOrder(int id);

        public Task<Order> UpdateOrder(Order order);
    }
}
