using System.Text;
using System.Text.Json;
using CustomerService.Application.Events;
using CustomerService.Application.Interfaces;
using RabbitMQ.Client;

namespace CustomerService.Infrastructure.Rabbit;

public class RabbitPublisher : IRabbitPublisher
{
    private readonly IModel _channel;
    private readonly IRabbitSettings _rabbitSettings;
    private readonly IBasicProperties _basicProperties;
    
    public RabbitPublisher(IRabbitSettings rabbitSettings)
    {
        _rabbitSettings = rabbitSettings;
        var connectionFactory = new ConnectionFactory
        {
            HostName = rabbitSettings.Host,
            Port = rabbitSettings.Port,
        };
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
        _basicProperties = _channel.CreateBasicProperties();
        _basicProperties.Persistent = true;

    }
    public void Publish(CustomerCreated customerCreated)
    {
            var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(customerCreated));
            _channel.BasicPublish(exchange: _rabbitSettings.Exchange, routingKey: _rabbitSettings.RoutingKey, body: message,basicProperties:_basicProperties);
            
    }
}