using BusinessObject.Models;
using BusinessObject.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IOrderService
    {
        public  Task<List<Order>> GetAllOrderAsync(int page, int pageSize, string? searchTerm);

        Task<Order> GetOrderById(int id);

        Task AddNewOrder(OrderRequestModel order);

        Task DeleteOrder(int id);

        Task<Order> UpdateOrder(int id,OrderRequestModel order);

        Task<List<Order>> GetOrdersByDateRange(DateTime startDate, DateTime closeDate);

        Task<List<Order>> SearchOrdersByUserId(int userId, int page = 1, int pageSize = 100, string? searchTerm = null);
    }
}
