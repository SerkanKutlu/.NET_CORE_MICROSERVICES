using System.Text;
using System.Text.Json;
using CustomerService.Application.Dto;
using CustomerService.Application.Interfaces;   
using RabbitMQ.Client;

namespace CustomerService.Infrastructure;

public class Publisher:IPublisher
{
    private readonly IModel _channel;
    public Publisher()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };
        var connection = factory.CreateConnection();
        
        _channel = connection.CreateModel();
    }

    public void Publish(CustomerForLogDTO customer)
    {
        const string exchangeName = "topicExchange";
        const string routingKey = "top.route";
        _channel.ExchangeDeclare(exchangeName, type: ExchangeType.Topic,durable:true);
        var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(customer));
        var basicProperties = _channel.CreateBasicProperties();
        basicProperties.Persistent = true;
        _channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, body: message);
    }
}