﻿using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Application.Interfaces;
using OrderService.Application.Models;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Services;

public class OrderRequestService:IOrderRequestService
{
    private readonly IMapper _mapper;
    private readonly ILogger<OrderRequestService> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderHelper _orderHelper;
    private readonly IPublisher _publisher;
    public OrderRequestService(IMapper mapper, ILogger<OrderRequestService> logger, IOrderRepository orderRepository, IOrderHelper orderHelper, IPublisher publisher)
    {
        _mapper = mapper;
        _logger = logger;
        _orderRepository = orderRepository;
        _orderHelper = orderHelper;
        _publisher = publisher;
    }


    public async Task<PagedList<Order>> GetAll(RequestParameters requestParameters, HttpContext context)
    {
        var orders = await _orderRepository.GetAll(requestParameters);
        context.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(orders.MetaData));
        _logger.LogInformation("Getting all orders data's from database");
        return orders;
    }

    public async Task<Order> GetById(string id)
    {
        var order = await _orderRepository.GetWithId(id);
        _logger.LogInformation($"Getting order data with {id} from database");
        return order;
    }

    public async Task<PagedList<Order>> GetOrdersOfCustomers(string customerId, RequestParameters requestParameters, HttpContext context)
    {
        var orders =await _orderRepository.GetOrdersOfCustomer(customerId,requestParameters);
        context.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(orders.MetaData));
        _logger.LogInformation($"Orders of customer with id {customerId} fetched");
        return orders;
    }

    public async Task<string> CreateOrder(OrderForCreationDto newOrder, HttpContext context)
    {
        var order = _mapper.Map<Order>(newOrder);
            
        await _orderHelper.SetTotalAmount(order);
        await _orderHelper.SetAddressOfOrder(order);
            
        await _orderRepository.CreateAsync(order);
        context.Response.Headers.Add("location",$"https://{context.Request.Headers["Host"]}/api/Orders/{order.Id}");
        _logger.LogInformation($"New order added with id {order.Id}");
        await _publisher.PublishOrderCreatedEvent(order);
        return order.Id;
    }

    public async Task<Order> UpdateOrder(OrderForUpdateDto newOrder)
    {
        var order = _mapper.Map<Order>(newOrder);
        await _orderHelper.SetPersistentDataForUpdate(order);
        await _orderHelper.SetTotalAmount(order);
        await _orderRepository.UpdateAsync(order);
        _logger.LogInformation($"Order with id {order.Id} updated.");
        await _publisher.PublishOrderUpdatedEvent(order);
        return order;
    }

    public async Task UpdateStatus(string id, string newStatus)
    {
        await _orderRepository.ChangeStatus(id, newStatus);
    }

    public async Task DeleteOrder(string id)
    {
        await _orderRepository.DeleteAsync(id);
        _logger.LogInformation($"Order with id {id} deleted.");
    }

    public async Task DeleteOrderOfCustomer(string customerId)
    {
        var result = await _orderRepository.DeleteOrderOfCustomer(customerId);
        if (!result) throw new NotFoundException<Order>();
        _logger.LogInformation($"Orders of customer with customer id {customerId} deleted.");
    }
}