using Common.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace OrderService.Application.Consumers;

public class OrderCreatedConsumer:ConsumerBase<OrderCreatedConsumer>,IConsumer<IOrderCreated>
{
    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger) : base(logger)
    {
    }

    public async Task Consume(ConsumeContext<IOrderCreated> context)
    {
        await Task.Run(() =>
        {
            var message = context.Message;
            Logger.LogInformation(message.LogMessage);
            Console.WriteLine("application consumer worked");
        });
    }
}