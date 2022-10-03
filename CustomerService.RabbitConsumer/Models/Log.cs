using CustomerService.Application.Events;
using GenericMongo.Bases;

namespace CustomerService.RabbitConsumer.Models;

public class Log : BaseEntity
{
    private string? _logMessage;
    public void FillLogMessage(CustomerCreated? createdEvent)
    {
        _logMessage = $"Customer Created: {createdEvent?.Id}, {createdEvent?.Address} ...";
    }
}

