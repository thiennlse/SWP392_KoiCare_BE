using BusinessObject.Models;
using BusinessObject.RequestModel;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IHttpContextAccessor contextAccessor)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _contextAccessor = contextAccessor;
        }



        public async Task<List<Order>> GetAllOrderAsync(int page, int pageSize, string? searchTerm)
        {
            return await _orderRepository.GetAllOrderAsync(page, pageSize, searchTerm);
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _orderRepository.GetById(id);
        }

        public async Task AddNewOrder(OrderRequestModel newOrder)
        {
            Order order = MapToOrder(newOrder);
            await _orderRepository.AddNewOrder(order);
        }

        public async Task DeleteOrder(int id)
        {
            _orderRepository.DeleteOrder(id);
        }

        public async Task<Order> UpdateOrder(int id, OrderRequestModel newOrder)
        {
            var order = MapToOrder(newOrder);
            order.Id = id;
            return await _orderRepository.UpdateOrder(order);
        }

        public async Task<List<Order>> GetOrdersByDateRange(DateTime startDate, DateTime closeDate)
        {
            int defaultPage = 1; // Assuming default page is 1
            int pageSize = 100;  // Assuming you're loading 100 records per page
            string? searchTerm = null; // No specific search term, so you can pass null

            // Fetch orders using pagination and the search term
            var allOrders = await _orderRepository.GetAllOrderAsync(defaultPage, pageSize, searchTerm);

            // Filter orders that match the given start and close dates
            var filteredOrders = allOrders
                .Where(order => order.OrderDate.Date >= startDate.Date && order.OrderDate.Date <= closeDate.Date)
                .ToList();

            return filteredOrders;
        }
        private Order MapToOrder(OrderRequestModel request)
        {


            List<Product> products = new List<Product>();
            foreach (var item in request.ProductId)
            {
                var product = _productRepository.getById(item);
                products.Add(product);
            }

            var user = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userid = int.Parse(user);
            return new Order
            {
                MemberId = userid,
                TotalCost = request.TotalCost,
                CloseDate = request.CloseDate,
                Code = request.Code,
                Description = request.Description,
                Status = request.Status,
                Product = products
            };
        }
    }
}
