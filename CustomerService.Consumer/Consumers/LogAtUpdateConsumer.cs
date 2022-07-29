
using Common.Events;
using MassTransit;

namespace CustomerService.Consumer.Consumers;

public class LogAtUpdateConsumer:ConsumerBase<LogAtUpdateConsumer>,IConsumer<ICustomerUpdated>
{
    public LogAtUpdateConsumer(ILogger<LogAtUpdateConsumer> logger) : base(logger)
    {
    }
    
    public Task Consume(ConsumeContext<ICustomerUpdated> context)
    {
        var message = context.Message;
        Logger.LogInformation(message.LogMessage);
        Console.WriteLine(message.LogMessage);
        return Task.CompletedTask;
    }
}