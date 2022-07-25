﻿using OrderService.Common.Models;
using OrderService.Entity.Models;

namespace OrderService.Repository.Repository.Interfaces;

public interface IOrderRepository
{
    Task CreateAsync(Order newOrder);
    Task UpdateAsync(Order updatedOrder);
    Task DeleteAsync(string orderId);
    Task<PagedList<Order>> GetAll(RequestParameters orderParameters);
    Task<Order> GetWithId(string orderId);
    Task ChangeStatus(string orderId, string newStatus);
    Task<PagedList<Order>> GetOrdersOfCustomer(string customerId,RequestParameters requestParameters);
    Task DeleteOrderOfCustomer(string customerId);
    Task UpdateProductRelatedOrders(string productId);
}