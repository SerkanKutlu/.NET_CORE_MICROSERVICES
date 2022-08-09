using Confluent.Kafka;
using OrderService.Application.DTO;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure;

public class Publisher:IPublisher
{
    private readonly IProducer<Null, OrderForLogDto> _producer;
    public Publisher()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
        };
        _producer = new ProducerBuilder<Null, OrderForLogDto>(config).SetValueSerializer(new OrderForLogDto()).Build();
    }

    public async Task PublishForLog(OrderForLogDto customer)
    {
        var message = new Message<Null, OrderForLogDto>
        {
            Value = customer
        };
        await _producer.ProduceAsync("orderLogging", message);
    }
}