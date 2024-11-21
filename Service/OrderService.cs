using BusinessObject.Models;
using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using iText.Forms.Form.Element;
using Microsoft.AspNetCore.Http;
using Repository.Interface;
using Service.Interface;
using System.Security.Claims;

namespace Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISubcriptionRepository _subcriptionRepository;
        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IHttpContextAccessor contextAccessor, IMemberRepository memberRepository, ISubcriptionRepository subcriptionRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _contextAccessor = contextAccessor;
            _memberRepository = memberRepository;
            _subcriptionRepository = subcriptionRepository;
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
            Order order = await _orderRepository.GetById(id);
            var existOrder = await MapToOrder(newOrder);
            var updatedOrder = await UpdataOrderDetails(existOrder, order);
            await _orderRepository.UpdateOrder(updatedOrder);
        }

        private async Task<Order> UpdataOrderDetails(Order order, Order old)
        {
            
            old.OrderDate = order.OrderDate.ToUniversalTime();
            old.TotalCost = order.TotalCost;
            old.CloseDate = order.CloseDate.ToUniversalTime();
            old.Code = order.Code;
            old.Description = order.Description;
            old.Status = order.Status;
            old.OrderProducts = old.OrderProducts;
            return old;
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
                OrderDate = DateTime.Now.ToUniversalTime(),
                CloseDate = request.CloseDate.ToUniversalTime(),
                Code = request.Code,
                Description = request.Description,
                Status = request.Status,
                OrderProducts = new List<OrderProduct>()
            };

            if (request.SubcriptionId != 0)
            {
                var subscriptionProduct = await _subcriptionRepository.GetById(request.SubcriptionId.Value);
                if (subscriptionProduct != null)
                {
                    order.OrderProducts.Add(new OrderProduct
                    {
                        SubcriptionId = request.SubcriptionId.Value,
                        Quantity = 1
                    });
                }
                else
                {
                    throw new InvalidOperationException($"Sản phẩm {request.SubcriptionId.Value} không có.");
                }
            }
            else
            {
                for (int i = 0; i < request.ProductId.Count; i++)
                {
                    var product = await _productRepository.GetById(request.ProductId[i].Value);
                    if (product != null && request.Quantity[i] > 0)
                    {
                        if (product.InStock >= request.Quantity[i])
                        {
                            product.InStock -= request.Quantity[i];
                            order.OrderProducts.Add(new OrderProduct
                            {
                                ProductId = product.Id,
                                Quantity = request.Quantity[i]
                            });
                            await _productRepository.UpdateProduct(product);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Số lượng tồn kho của sản phẩm {product.Id} không đủ.");
                        }
                    }
                }
            }
            return order;
        }

        public async Task<OrderProductResponse> GetProductByOrderId(int orderId)
        {
            var user = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userid = int.Parse(user);
            var order = await _orderRepository.GetProductByOrderId(orderId,userid);
            var orderProduct = order.OrderProducts.Select(o => o.ProductId.Value).ToList();
            var products = order.OrderProducts.Select(o => o.Product).ToList();
            var quantity = order.OrderProducts.Select(o => o.Quantity);
            var total = order.OrderProducts
                 .Where(op => op.Product != null)
                 .Sum(op => op.Product.Cost * op.Quantity);

            return new OrderProductResponse
            {
                Amount = total,
                ProductId = orderProduct
            };
        }
    }
}
