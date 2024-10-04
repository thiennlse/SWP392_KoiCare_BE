using BusinessObject.Models;
using Repository;
using Repository.Interface;
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
            try
            {
                return await _orderRepository.GetAllOrder();
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error retrieving all orders.", ex);
            }
        }

        public async Task<Order> GetOrderById(int id)
        {
            try
            {
                return await _orderRepository.GetOrderById(id);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"Error retrieving the order with ID {id}.", ex);
            }
        }

        public async Task AddNewOrder(Order newOrder)
        {
            try
            {
                await _orderRepository.AddNewOrder(newOrder); // Ensure AddNewOrder method exists in repository
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error adding new order.", ex);
            }
        }

        public async Task DeleteOrder(int id)
        {
            try
            {
                await _orderRepository.DeleteOrder(id); // Marking it async and adding await
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception($"Error deleting the order with ID {id}.", ex);
            }
        }

        public async Task<Order> UpdateOrder(Order newOrder)
        {
            try
            {
                return await _orderRepository.UpdateOrder(newOrder);
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error updating the order.", ex);
            }
        }
    }
}
