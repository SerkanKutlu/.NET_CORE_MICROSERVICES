using Common.Events;
using MassTransit;

namespace CustomerService.Consumer.Consumers;

public class LogAtCreateConsumer:ConsumerBase<LogAtCreateConsumer>, IConsumer<ICustomerCreated>
{
    public LogAtCreateConsumer(ILogger<LogAtCreateConsumer> logger) : base(logger)
    {
        
    }

    public Task Consume(ConsumeContext<ICustomerCreated> context)
    {
        var message = context.Message;
        Logger.LogInformation(message.LogMessage);
        Console.WriteLine(message.LogMessage);
        return Task.CompletedTask;
    }
}