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

        Task UpdateOrder(int id,OrderRequestModel order);

        Task<List<Order>> GetOrdersByOrderDateRange(DateTime startDate, DateTime closeDate);

        Task<List<Order>> GetOrdersByCloseDateRange(DateTime startDate, DateTime closeDate);

        Task<List<Order>> GetOrdersByOrderDateAndCloseDate(DateTime OrderDate, DateTime CloseDate);

        Task<List<Order>> SearchOrdersByUserId(int id, int page = 1, int pageSize = 100, string? searchTerm = null);
    }
}
