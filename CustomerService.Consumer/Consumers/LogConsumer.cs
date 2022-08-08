using System.Text;
using System.Text.Json;
using CustomerService.Application.Dto;
using CustomerService.Consumer.Interfaces;
using CustomerService.Consumer.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CustomerService.Consumer.Consumers;

public class LogConsumer :ConsumerBase
{
    private int _bulkSize;
    private readonly List<BasicDeliverEventArgs> _messageList;
    private readonly List<Log> _logList;
    public LogConsumer(string exchangeName, string routingKey,string queueName,IConsumerService consumerService)
        : base(exchangeName,routingKey,queueName,consumerService)
    {
        _logList = new List<Log>();
        _bulkSize = 0;
        _messageList = new List<BasicDeliverEventArgs>();
    }
    
    public void StartConsumer()
    {

        Consumer.Received += (model,ea) =>
        {
            Console.WriteLine("HERE");
            _messageList.Add(ea);
            var x = model;
            _bulkSize++;
             if (_bulkSize != 3) return;
             _bulkSize = 0;
             foreach (var message in _messageList)
             {
                var stringMessage = Encoding.UTF8.GetString(message.Body.ToArray());
                var customerForLog = JsonSerializer.Deserialize<CustomerForLogDto>(stringMessage);
                var logMessage = $"A customer is {customerForLog?.Action}. {customerForLog}";
                _logList.Add(new Log{LogMessage = logMessage});
             }
             ConsumerService.LogMany(_logList);
             Channel.BasicAck(ea.DeliveryTag, true);
            _logList.Clear();
            _messageList.Clear();
        };
        Channel.BasicConsume(queue: QueueName, consumer: Consumer, autoAck: false);
   
    }
}