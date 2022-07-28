using CustomerService.Domain.Entities;
using MassTransit;

namespace CustomerService.Consumer.Consumers;

public class CreateLogConsumer:ConsumerBase<UpdateLogConsumer>,IConsumer<Customer>
{
    public CreateLogConsumer(ILogger<UpdateLogConsumer> logger) : base(logger)
    {
    }

    public async Task Consume(ConsumeContext<Customer> context)
    {
        await Task.Run(() =>
        {
            var data = context.Message;
            var log = $"New Customer Added:\nId: {data.Id}\nEmail: {data.Email}\nCreate:{data.CreatedAt}\n";
            Logger.LogInformation(log);
            Console.WriteLine(log);
        });
    }
}