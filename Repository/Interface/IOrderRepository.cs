using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<List<Order>> GetAllOrder();

        Task AddNewOrder(Order order);

        Task DeleteOrder(int id);
        Task<Order> UpdateOrder(Order order);
    }
}
