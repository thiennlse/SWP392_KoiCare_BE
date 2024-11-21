using BusinessObject.Models;
using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;

namespace Service.Interface
{
    public interface IOrderService
    {
        public Task<List<Order>> GetAllOrderAsync(int page, int pageSize, string? searchTerm);

        Task<Order> GetOrderById(int id);

        Task AddNewOrder(OrderRequestModel order);

        Task DeleteOrder(int id);

        Task UpdateOrder(int id, OrderRequestModel order);

        Task<OrderProductResponse> GetProductByOrderId(int orderId);
    }
}
