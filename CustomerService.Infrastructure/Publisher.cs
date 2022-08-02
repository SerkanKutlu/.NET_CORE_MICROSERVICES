using System.Text;
using System.Text.Json;
using CustomerService.Application.Interfaces;
using CustomerService.Domain.Entities;
using RabbitMQ.Client;

namespace CustomerService.Infrastructure;

public class Publisher:IPublisher
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    
    public Publisher()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }
    public void Publish(Customer customer)
    {
        const string exchangeName = "topicExchange";
        const string routingKey = "top.route";
        _channel.ExchangeDeclare(exchangeName, type: ExchangeType.Topic,durable:true);

        
        var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(customer));
        var basicProperties = _channel.CreateBasicProperties();
        basicProperties.Persistent = true;
        _channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, body: message);
        _connection.Close();
    }
}