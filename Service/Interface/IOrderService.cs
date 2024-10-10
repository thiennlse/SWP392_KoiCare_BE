using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrder();

        Task<Order> GetOrderById(int id);

        Task AddNewOrder(Order order);

        Task DeleteOrder(int id);

        Task<Order> UpdateOrder(Order order);

        Task<Order> CalculateOrderdate(int orderId);

        Task<List<Order>> GetOrdersByDateRange(DateTime orderDate, DateTime closeDate);
    }
}
