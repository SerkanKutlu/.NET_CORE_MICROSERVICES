namespace CustomerService.KafkaConsumer;

public class Worker : BackgroundService
{

    private readonly Consumers.KafkaConsumer _consumer;

    public Worker(Consumers.KafkaConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await _consumer.StartMainConsume();
        }
    }
}