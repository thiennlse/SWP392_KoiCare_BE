﻿using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        public  Task<List<Order>> GetAllOrderAsync(int page, int pageSize, string? searchTerm);
        public Task<Order> GetOrderByCode(string orderCode);
        Task AddNewOrder(Order order);
        Task DeleteOrder(int id);
        Task<Order> UpdateOrder(Order order);
        Order getById(int id);
        Task<Order> GetProductByOrderId(int orderId, int userId);
    }
}
