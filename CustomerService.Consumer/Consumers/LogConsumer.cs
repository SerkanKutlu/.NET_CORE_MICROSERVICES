using System.Text;
using System.Text.Json;
using CustomerService.Application.Dto;
using RabbitMQ.Client;
namespace CustomerService.Consumer.Consumers;

public class LogConsumer :ConsumerBase<LogConsumer>
{
    public LogConsumer(string exchangeName, string routingKey,string queueName,ILogger<LogConsumer> logger) : base(exchangeName,routingKey,queueName,logger)
    {
    }
    
    public void StartConsumer()
    {
        Consumer.Received += (model,ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            var customerForLog = JsonSerializer.Deserialize<CustomerForLogDTO>(message);
            var logMessage = $"A customer is {customerForLog?.Action}. {customerForLog}";
            Logger.LogInformation(logMessage);
            Channel.BasicAck(ea.DeliveryTag, false);
        };
        Channel.BasicConsume(queue: QueueName, consumer: Consumer, autoAck: false);
    }
}