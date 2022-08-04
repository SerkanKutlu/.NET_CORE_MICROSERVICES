using System.Text;
using System.Text.Json;
using CustomerService.Application.Dto;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CustomerService.Consumer.Consumers;

public class LogConsumer :ConsumerBase<LogConsumer>
{
    private int _bulkSize;
    private readonly IList<BasicDeliverEventArgs> _messageList;
    public LogConsumer(string exchangeName, string routingKey,string queueName,ILogger<LogConsumer> logger) : base(exchangeName,routingKey,queueName,logger)
    {
        _bulkSize = 0;
        _messageList = new List<BasicDeliverEventArgs>();
    }
    
    public void StartConsumer()
    {
        Consumer.Received += (model,ea) =>
        {
            _messageList.Add(ea);
            _bulkSize++;
            if (_bulkSize != 3) return;
            _bulkSize = 0;
            foreach (var message in _messageList)
            {
                Console.WriteLine("veri geldi");
                var stringMessage = Encoding.UTF8.GetString(message.Body.ToArray());
                var customerForLog = JsonSerializer.Deserialize<CustomerForLogDto>(stringMessage);
                var logMessage = $"A customer is {customerForLog?.Action}. {customerForLog}";
                Logger.LogInformation(logMessage);
                Channel.BasicAck(message.DeliveryTag, false);
            }
            _messageList.Clear();
            
            
        };
   
    }
}