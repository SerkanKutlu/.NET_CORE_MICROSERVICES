using CustomerService.Consumer.Consumers;

namespace CustomerService.Consumer;

public class Worker : BackgroundService
{
    private readonly LogConsumer _logConsumer;
    public Worker(LogConsumer logConsumer)
    {
        _logConsumer = logConsumer;
      
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            _logConsumer.StartConsumer();
        },stoppingToken);
        
        
        while (!stoppingToken.IsCancellationRequested)
        {
            
        }
    }
}

