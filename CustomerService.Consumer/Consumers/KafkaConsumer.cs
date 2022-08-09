using Confluent.Kafka;
using CustomerService.Application.Dto;
using CustomerService.Consumer.Interfaces;
using CustomerService.Consumer.Models;
using MongoDB.Driver;

namespace CustomerService.Consumer.Consumers;

public class KafkaConsumer
{
    private readonly IConsumer<Ignore, CustomerForLogDto> _consumer;
    private readonly IMongoCollection<Log> _logs;
    public KafkaConsumer(IMongoService mongoService)
    {
        _logs = mongoService.Logs;
        var config = new ConsumerConfig
        {
            
            BootstrapServers = "localhost:9092",
            GroupId = "consumers1",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, CustomerForLogDto>(config).SetValueDeserializer(new CustomerForLogDto()).Build();
        _consumer.Subscribe("topic1");
    }


    public void StartConsume()
    {
        var consumeResult = _consumer.Consume();
        var data = consumeResult.Message.Value;
        // _logs.InsertOne(new Log
        // {
        //     LogMessage = $"A customer is {data.Action}.  {data}"
        // });
        Console.WriteLine($"FROM CONSUMER 1{data.Action}.  {data}");
        
    }
}