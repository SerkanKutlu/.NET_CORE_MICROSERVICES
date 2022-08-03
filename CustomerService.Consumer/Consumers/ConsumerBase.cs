//using RabbitMQ.Client;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CustomerService.Consumer.Consumers;

public abstract class ConsumerBase<T>
{
    protected readonly IModel Channel;
    protected readonly string QueueName; 
    protected readonly EventingBasicConsumer Consumer;
    protected readonly ILogger<T> Logger;

    protected ConsumerBase(string exchangeName, string routingKey,string queueName, ILogger<T> logger)
    {
        Logger = logger;
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };
        var connection = connectionFactory.CreateConnection();
        Channel = connection.CreateModel();
        Channel.ExchangeDeclare(exchangeName, type: ExchangeType.Topic,durable:true);
        QueueName = Channel.QueueDeclare(queueName,exclusive:false,autoDelete:false,durable:true).QueueName;
        Channel.QueueBind(QueueName,exchangeName,routingKey);
        Consumer = new EventingBasicConsumer(Channel);
    }
}