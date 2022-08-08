﻿using System.Text;
using System.Text.Json;
using CustomerService.Application.Dto;
using CustomerService.Application.Interfaces;   
using RabbitMQ.Client;

namespace CustomerService.Infrastructure;

public class Publisher:IPublisher
{
    private readonly IModel _channel;
    private const string ExchangeName = "customer.log";
    private string _routingKey;
    private readonly IBasicProperties _basicProperties;
    public Publisher()
    {
        var factory = new ConnectionFactory() {HostName = "localhost"};
        factory.Port = 5672;
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _basicProperties = _channel.CreateBasicProperties();
        _basicProperties.Persistent = true;
    }

    public void PublishForLog(CustomerForLogDto customer)
    {
        _routingKey = "customer.log";
        var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(customer));
        _channel.BasicPublish(exchange: ExchangeName, routingKey: _routingKey, body: message,basicProperties:_basicProperties);
    }
}