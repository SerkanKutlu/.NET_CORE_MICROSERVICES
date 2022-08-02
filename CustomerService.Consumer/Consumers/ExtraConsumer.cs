using System.Text;
using RabbitMQ.Client;
namespace CustomerService.Consumer.Consumers;

public class ExtraConsumer:ConsumerBase, IConsumer
{
    public ExtraConsumer(string exchangeName, string routingKey, string queueName) : base(exchangeName, routingKey,queueName)
    {
    }

    public Task StartConsumer()
    {
        Consumer.Received += (model,ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("received from extra consumer", message);
            //Channel.BasicAck(ea.DeliveryTag, false);
        };
        Channel.BasicConsume(queue: QueueName, consumer: Consumer, autoAck: false);
        return Task.CompletedTask;
    }
}