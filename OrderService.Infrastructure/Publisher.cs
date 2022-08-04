using System.Text;
using System.Text.Json;
using OrderService.Application.DTO;
using OrderService.Application.Interfaces;
using RabbitMQ.Client;

namespace OrderService.Infrastructure;

public class Publisher:IPublisher
{
    private readonly IModel _channel;
    private const string ExchangeName = "orderExchange";
    private string _routingKey;
    private readonly IBasicProperties _basicProperties;
    public Publisher()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _basicProperties = _channel.CreateBasicProperties();
        _basicProperties.Persistent = true;
    }

    public void PublishForLog(OrderForLogDto order)
    {
        _routingKey = "order.log";
        var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(order));
        _channel.BasicPublish(exchange: ExchangeName, routingKey: _routingKey, body: message,basicProperties:_basicProperties);
    }
}