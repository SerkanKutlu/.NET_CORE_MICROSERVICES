namespace CustomerService.RabbitConsumer;

public class Worker : BackgroundService
{
    private readonly Consumers.RabbitConsumer _consumer;

    public Worker(Consumers.RabbitConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.StartConsumer();
        while (!stoppingToken.IsCancellationRequested)
        {
            
        }

        return Task.CompletedTask;
    }
}