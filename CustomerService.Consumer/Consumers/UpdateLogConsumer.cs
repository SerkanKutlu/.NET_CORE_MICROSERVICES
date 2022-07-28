using CustomerService.Domain.Entities;
using MassTransit;

namespace CustomerService.Consumer.Consumers;

public class UpdateLogConsumer:ConsumerBase<UpdateLogConsumer>,IConsumer<Customer>
{
    public UpdateLogConsumer(ILogger<UpdateLogConsumer> logger) : base(logger)
    {
    }
    
    
    public async Task Consume(ConsumeContext<Customer> context)
    {
        await Task.Run(() =>
        {
            var data = context.Message;
            var log = $"A Customer Updated:\nId: {data.Id}\nEmail: {data.Email}\nCreate:{data.CreatedAt}\nUpdate:{data.UpdatedAt}\n";
            Logger.LogInformation(log);
            Console.WriteLine(log);
        });
        
    }

    
}