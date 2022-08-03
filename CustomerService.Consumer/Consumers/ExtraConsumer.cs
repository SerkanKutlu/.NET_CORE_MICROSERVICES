using System.Text;
using RabbitMQ.Client;
namespace CustomerService.Consumer.Consumers;

public class ExtraConsumer:ConsumerBase<ExtraConsumer>
{
    public ExtraConsumer(string exchangeName, string routingKey, string queueName, ILogger<ExtraConsumer> logger) : base(exchangeName, routingKey,queueName,logger)
    {
    }

    public void StartConsumer()
    {
        Consumer.Received += (model,ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("received from extra consumer", message);
            Channel.BasicAck(ea.DeliveryTag, true);
        };
        Channel.BasicConsume(queue: QueueName, consumer: Consumer, autoAck: false);
    }
}