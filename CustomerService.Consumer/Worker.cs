using CustomerService.Consumer.Consumers;
using CustomerService.Consumer.Interfaces;
using CustomerService.Consumer.Models;
using MongoDB.Driver;

namespace CustomerService.Consumer;

public class Worker : BackgroundService
{

    private readonly KafkaConsumer _consumer;
    private readonly KafkaConsumer2 _consumer2;
    
    public Worker(KafkaConsumer consumer, KafkaConsumer2 consumer2)
    {
        _consumer = consumer;
        _consumer2 = consumer2;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        while (!stoppingToken.IsCancellationRequested)
        {
            _consumer.StartConsume();
            _consumer2.StartConsume2();
        }
    }
}

