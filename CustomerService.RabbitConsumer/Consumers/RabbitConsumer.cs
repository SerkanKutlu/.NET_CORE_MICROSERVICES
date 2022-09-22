using System.Text;
using System.Text.Json;
using CustomerService.Application.Events;
using CustomerService.Application.Interfaces;
using CustomerService.RabbitConsumer.Models;
using CustomerService.RabbitConsumer.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CustomerService.RabbitConsumer.Consumers;

public class RabbitConsumer
{
    private readonly EventingBasicConsumer _consumer;
    private readonly LogRepository _logRepository;
    private int _bulkSize;
    private readonly List<Log> _logList;
    private readonly IModel _channel;
    private readonly IRabbitSettings _rabbitSettings;
    
    public RabbitConsumer(IRabbitSettings rabbitSettings, LogRepository logRepository)
    {
        _rabbitSettings = rabbitSettings;
        _logRepository = logRepository;
        _logList = new List<Log>();
        _bulkSize = 0;
        var connectionFactory = new ConnectionFactory
        {
            HostName = rabbitSettings.Host,
            Port = rabbitSettings.Port
        };
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
        _consumer = new EventingBasicConsumer(_channel);
    }
    
    public void StartConsumer()
    {
        _consumer.Received += (model, ea) =>
        {
            _bulkSize++;
            var stringMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
            var customerCreated = JsonSerializer.Deserialize<CustomerCreated>(stringMessage);
            var log = new Log();
            log.FillLogMessage(customerCreated);
            _logList.Add(log);
            if (_bulkSize != 3) return;
            _bulkSize = 0;
            _logRepository.AddRangeAsync(_logList).Wait();
            _channel.BasicAck(ea.DeliveryTag, true);
            _logList.Clear();
            
        };
        _channel.BasicConsume(queue:_rabbitSettings.QueueName , consumer: _consumer, autoAck: false);
    }
}
