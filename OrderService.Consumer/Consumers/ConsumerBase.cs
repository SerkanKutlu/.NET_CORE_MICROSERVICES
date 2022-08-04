using OrderService.Consumer.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.Consumer.Consumers;

public abstract class ConsumerBase
{
    protected readonly IModel Channel;
    protected readonly string QueueName; 
    protected readonly EventingBasicConsumer Consumer;
    protected readonly IConsumerService ConsumerService;

    protected ConsumerBase(string exchangeName, string routingKey,string queueName, IConsumerService consumerService)
    {
        ConsumerService = consumerService;
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };
        var connection = connectionFactory.CreateConnection();
        Channel = connection.CreateModel();
        QueueName = queueName;
        //Channel.QueueBind(queueName,exchangeName,routingKey);
        Consumer = new EventingBasicConsumer(Channel);
    }
    
}