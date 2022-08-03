﻿using Common.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace OrderService.Application.Consumers;

public class OrderUpdatedConsumer : ConsumerBase<OrderUpdatedConsumer>,IConsumer<IOrderUpdated>
{
    public OrderUpdatedConsumer(ILogger<OrderUpdatedConsumer> logger) : base(logger)
    {
    }
    
    public async Task Consume(ConsumeContext<IOrderUpdated> context)
    {
        await Task.Run(() =>
        {
            var message = context.Message;
            Logger.LogInformation(message.LogMessage);
            Console.WriteLine(message.LogMessage);
        });
    }
}