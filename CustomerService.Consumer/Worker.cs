using CustomerService.Consumer.Consumers;

namespace CustomerService.Consumer;

public class Worker : BackgroundService
{
    private readonly LogConsumer _logConsumer;
    private readonly ExtraConsumer _extraConsumer;
    public Worker(LogConsumer logConsumer, ExtraConsumer extraConsumer)
    {
        _logConsumer = logConsumer;
        _extraConsumer = extraConsumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            _logConsumer.StartConsumer();
        },stoppingToken);
        await Task.Run(() =>
        {
            _extraConsumer.StartConsumer();
        },stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            
        }
    }
}

