using CustomerService.Consumer.Consumers;

namespace CustomerService.Consumer;

public class Worker : BackgroundService
{
   // private readonly ILogger<Worker> _logger;
    public Worker()//ILogger<Worker> logger)
    {
        //_logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      
        IConsumer consumer = new LogConsumer("topicExchange", "top.route","queue1");
       // IConsumer extraConsumer = new ExtraConsumer("topicExchange", "top.route","extra_queue");
        await consumer.StartConsumer();
        //await extraConsumer.StartConsumer();
        while (!stoppingToken.IsCancellationRequested)
        {
            
        }
    }
}

