using System.Net;
using Confluent.Kafka;
using CustomerService.Application.Dto;
using CustomerService.Application.Interfaces;
namespace CustomerService.Infrastructure;

public class Publisher:IPublisher
{
    private readonly IProducer<Null, CustomerForLogDto> _producer;
    public Publisher()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            ClientId = Dns.GetHostName()
        };
        _producer = new ProducerBuilder<Null, CustomerForLogDto>(config).SetValueSerializer(new CustomerForLogDto()).Build();
    }

    public async Task PublishForLog(CustomerForLogDto customer)
    {
        var message = new Message<Null, CustomerForLogDto>
        {
            Value = customer
        };
        await _producer.ProduceAsync("topic1", message);
    }
}