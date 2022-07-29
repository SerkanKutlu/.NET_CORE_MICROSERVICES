using Common.Events;
using MassTransit;


namespace OrderService.Consumer.Consumers;

public class OrderCreatedLogConsumer:ConsumerBase<OrderCreatedLogConsumer>,IConsumer<IOrderCreated>
{
    public OrderCreatedLogConsumer(ILogger<OrderCreatedLogConsumer> logger) : base(logger)
    {
    }

    public async Task Consume(ConsumeContext<IOrderCreated> context)
    {
        await Task.Run(() =>
        {
            var message = context.Message;
            Logger.LogInformation(message.LogMessage);
            Console.WriteLine("proje worked");
        });
    }
}