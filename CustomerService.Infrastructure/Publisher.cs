using System.Text;
using System.Text.Json;
using CustomerService.Application.Dto;
using CustomerService.Application.Interfaces;   
using RabbitMQ.Client;

namespace CustomerService.Infrastructure;

public class Publisher:IPublisher
{
    private readonly IModel _channel;
    private const string ExchangeName = "customerExchange";
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
        //_channel.ExchangeDeclare(ExchangeName, type: ExchangeType.Topic,durable:true);
        _basicProperties = _channel.CreateBasicProperties();
        _basicProperties.Persistent = true;
    }

    public void PublishForLog(CustomerForLogDto customer)
    {
        _routingKey = "customer.log";
        var x = JsonSerializer.Serialize(customer);
        var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(customer));
        _channel.BasicPublish(exchange: ExchangeName, routingKey: _routingKey, body: message,basicProperties:_basicProperties);
    }
}