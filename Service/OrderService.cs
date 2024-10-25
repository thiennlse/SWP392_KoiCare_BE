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
        private readonly IMemberRepository _memberRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IHttpContextAccessor contextAccessor, IMemberRepository memberRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _contextAccessor = contextAccessor;
            _memberRepository = memberRepository;
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
            Order order = await MapToOrder(newOrder);
            await _orderRepository.AddNewOrder(order);
        }

        public async Task DeleteOrder(int id)
        {
            _orderRepository.DeleteOrder(id);
        }

        public async Task UpdateOrder(int id, OrderRequestModel newOrder)
        {
            var order = await MapToOrder(newOrder);
            order.Id = id;
            await _orderRepository.UpdateOrder(order);
        }

        public async Task<List<Order>> GetOrdersByOrderDateRange(DateTime startDate, DateTime closeDate)
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

        public async Task<List<Order>> GetOrdersByCloseDateRange(DateTime startDate, DateTime closeDate)
        {
            int defaultPage = 1; // Assuming default page is 1
            int pageSize = 100;  // Assuming you're loading 100 records per page
            string? searchTerm = null; // No specific search term, so you can pass null

            // Fetch orders using pagination and the search term
            var allOrders = await _orderRepository.GetAllOrderAsync(defaultPage, pageSize, searchTerm);

            // Filter orders that match the given start and close dates
            var filteredOrders = allOrders
                .Where(order => order.CloseDate.Date >= startDate.Date && order.CloseDate.Date <= closeDate.Date)
                .ToList();

            return filteredOrders;
        }

        public async Task<List<Order>> GetOrdersByOrderDateAndCloseDate(DateTime startOrderDate, DateTime endOrderDate, DateTime startCloseDate, DateTime endCloseDate)
        {
            // Assuming default pagination parameters for fetching orders
            int defaultPage = 1;
            int pageSize = 100;
            string? searchTerm = null;

            // Fetch orders using pagination and the search term
            var allOrders = await _orderRepository.GetAllOrderAsync(defaultPage, pageSize, searchTerm);

            // Filter orders that match both date ranges
            var filteredOrders = allOrders
                .Where(order =>
                    order.OrderDate.Date >= startOrderDate.Date && order.OrderDate.Date <= endOrderDate.Date &&
                    order.CloseDate.Date >= startCloseDate.Date && order.CloseDate.Date <= endCloseDate.Date)
                .ToList();

            return filteredOrders;
        }

        public async Task<List<Order>> SearchOrdersByUserId(int id, int page = 1, int pageSize = 100, string? searchTerm = null)
        {
            // Fetch orders using pagination and an optional search term
            var allOrders = await _orderRepository.GetAllOrderAsync(page, pageSize, searchTerm);

            // Filter orders by userId
            var filteredOrders = allOrders
                .Where(order => order.MemberId == id) // Use MemberId as the UserId field
                .ToList();

            return filteredOrders;
        }

        private async Task<Order> MapToOrder(OrderRequestModel request)
        {
            var user = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userid = int.Parse(user);

            var order = new Order
            {
                MemberId = userid,
                Member = await _memberRepository.GetById(userid),
                TotalCost = request.TotalCost,
                OrderDate = DateTime.Now,
                CloseDate = request.CloseDate,
                Code = GenerateUniqueCode(),
                Description = request.Description,
                Status = "PAID",
                OrderProducts = new List<OrderProduct>()
            };

            for (int i = 0; i < request.ProductId.Count; i++)
            {
                var product = await _productRepository.GetById(request.ProductId[i]);
                if (product != null && request.Quantity[i] > 0)
                {
                    // Trừ số lượng sản phẩm tồn kho
                    if (product.InStock >= request.Quantity[i])
                    {
                        product.InStock -= request.Quantity[i];
                        // Thêm sản phẩm vào đơn hàng
                        order.OrderProducts.Add(new OrderProduct
                        {
                            ProductId = product.Id,
                            Quantity = request.Quantity[i]
                        });

                        // Cập nhật sản phẩm
                        await _productRepository.UpdateProduct(product);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Số lượng tồn kho của sản phẩm {product.Id} không đủ.");
                    }
                }
            }
            return order;
        }
        private string GenerateUniqueCode() => Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
    }
}
