using CustomerService.Consumer.Consumers;


namespace CustomerService.Consumer;

public class Worker : BackgroundService
{

    private readonly KafkaConsumer _consumer;

    public Worker(KafkaConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        while (!stoppingToken.IsCancellationRequested)
        {
            _consumer.StartConsume();
        }
    }
}

