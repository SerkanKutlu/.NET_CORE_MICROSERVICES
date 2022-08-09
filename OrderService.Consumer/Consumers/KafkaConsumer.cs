using Confluent.Kafka;
using MongoDB.Driver;
using OrderService.Application.DTO;
using OrderService.Consumer.Interfaces;
using OrderService.Consumer.Models;

namespace OrderService.Consumer.Consumers;

public class KafkaConsumer
{
    private readonly IConsumer<Ignore, OrderForLogDto> _consumer;
    private readonly IMongoCollection<Log> _logs;
    private readonly List<ConsumeResult<Ignore,OrderForLogDto>> _messageList = new() ;
    private int _bulkSize;
    private const int BulkCapacity = 5;
    private readonly List<Log> _logList  = new();
    public KafkaConsumer(IMongoService mongoService)
    {
        _logs = mongoService.Logs;
        var config = new ConsumerConfig
        {
            
            BootstrapServers = "localhost:9092",
            GroupId = "orderLogConsumers",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            
        };

        _consumer = new ConsumerBuilder<Ignore, OrderForLogDto>(config).SetValueDeserializer(new OrderForLogDto()).Build();
        _consumer.Subscribe("orderLogging");
    }


    public void StartConsume()
    {
        var consumeResult = _consumer.Consume();
        _bulkSize++;
        _messageList.Add(consumeResult);
        _logList.Add(new Log
        {
            Id = Guid.NewGuid().ToString(),
            LogMessage = $"Order was {consumeResult.Message.Value.Action}.  {consumeResult.Message.Value}"
        });
        if (_bulkSize == BulkCapacity)
        {
            _logs.InsertMany(_logList);
            foreach (var message in _messageList)
            {
                _consumer.Commit(message);
            }
            _logList.Clear();
            _messageList.Clear();
            _bulkSize = 0;
        }
        

    }
}


