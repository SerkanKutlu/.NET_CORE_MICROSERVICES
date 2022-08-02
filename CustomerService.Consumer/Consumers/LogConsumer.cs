using System.Text;
using RabbitMQ.Client;
namespace CustomerService.Consumer.Consumers;

public class LogConsumer :ConsumerBase, IConsumer
{
    public LogConsumer(string exchangeName, string routingKey,string queueName) : base(exchangeName,routingKey,queueName)
    {
    }
    
    public Task StartConsumer()
    {
        Consumer.Received += (model,ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
            //Channel.BasicAck(ea.DeliveryTag, false);
        };
        Channel.BasicConsume(queue: QueueName, consumer: Consumer, autoAck: false);
        return Task.CompletedTask;
    }
}