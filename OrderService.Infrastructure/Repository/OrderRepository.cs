using MongoDB.Driver;
using OrderService.Application.Exceptions;
using OrderService.Application.Interfaces;
using OrderService.Application.Models;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Extensions;

namespace OrderService.Infrastructure.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _orders;

    public OrderRepository(IMongoService mongoService)
    {
        _orders = mongoService.Orders;
    }


    public async Task CreateAsync(Order newOrder)
    {
        await _orders.InsertOneAsync(newOrder);
    }


    public async Task UpdateAsync(Order updatedOrder)
    {
        var result = await _orders.ReplaceOneAsync(o => o.Id == updatedOrder.Id, updatedOrder);
        if (!result.IsModifiedCountAvailable && result.ModifiedCount == 0)
            throw new NotFoundException<Order>(updatedOrder.Id);
    }


    public async Task DeleteAsync(string orderId)
    {
        var result = await _orders.DeleteOneAsync(o => o.Id == orderId);
        if (result.DeletedCount == 0)
            throw new NotFoundException<Order>(orderId);
    }


    public async Task<PagedList<Order>> GetOrdersPaged(RequestParameters requestParameters)
    {
        var orders = await _orders
            .Search(requestParameters.SearchTerm)
            .CustomSort(requestParameters.OrderBy)
            .ToListAsync();
        var pagedOrders =
            PagedList<Order>.ToPagedList(orders, requestParameters.PageNumber, requestParameters.PageSize);
        if (!orders.Any())
            throw new NotFoundException<Order>();
        return pagedOrders;
    }


    public async Task<Order> GetWithId(string orderId)
    {
        var order = await _orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        if (order == null)
            throw new NotFoundException<Order>(orderId);
        return order;
    }


    public async Task ChangeStatus(string orderId, string newStatus)
    {
        var result = await _orders.UpdateOneAsync(
            o => o.Id == orderId,
            Builders<Order>.Update
                .Set(o => o.Status, newStatus));

        if (!result.IsModifiedCountAvailable && result.ModifiedCount == 0)
            throw new NotFoundException<Order>(orderId);
    }


    public async Task<PagedList<Order>> GetOrdersOfCustomer(string customerId, RequestParameters requestParameters)
    {
        var orders = await _orders
            .Search(requestParameters.SearchTerm, customerId)
            .CustomSort(requestParameters.OrderBy)
            .ToListAsync();
        var pagedOrders =
            PagedList<Order>.ToPagedList(orders, requestParameters.PageNumber, requestParameters.PageSize);
        if (!orders.Any())
            throw new NotFoundException<Order>();
        return pagedOrders;
    }

    public async Task<bool> DeleteOrderOfCustomer(string customerId)
    {
        var result = await _orders.DeleteManyAsync(o => o.CustomerId == customerId);
        return result.DeletedCount > 0;
    }

    public async Task UpdateProductRelatedOrders(Product product)
    {
        
        var builder = Builders<Order>.Filter;
        var filter = builder.AnyEq(o => o.ProductIds, product.Id);
        await _orders.Find(filter).ForEachAsync(async order =>
        {
            order.ProductIds.Remove(product.Id);
            if (order.ProductIds.Count == 0)
                await DeleteAsync(order.Id);
            else
            {
                order.Quantity--;
                order.Total -= product.Price;
                await UpdateAsync(order);
            }
                    
        });
    }

    
}