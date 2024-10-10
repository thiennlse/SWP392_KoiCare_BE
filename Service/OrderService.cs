using BusinessObject.Models;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }



        public async Task<List<Order>> GetAllOrder()
        {
            return await _orderRepository.GetAllOrder();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _orderRepository.GetById(id);
        }

        public async Task AddNewOrder(Order newOrder)
        {
            await _orderRepository.UpdateOrder(newOrder);
        }

        public async Task DeleteOrder(int id)
        {
            _orderRepository.DeleteOrder(id);
        }

        public async Task<Order> UpdateOrder(Order newOrder)
        {
            return await _orderRepository.UpdateOrder(newOrder);
        }
        public async Task<int> CalculateOrderdate(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order != null)
            {
                if (order.OrderDate != DateTime.MinValue && order.CloseDate != DateTime.MinValue)
                {
                    TimeSpan duration = order.CloseDate - order.OrderDate;
                    return duration.Days;
                }
                else
                {
                    throw new Exception("OrderDate or CloseDate is not set.");
                }
            }
            throw new Exception("Order not found");
        }
    }
}
