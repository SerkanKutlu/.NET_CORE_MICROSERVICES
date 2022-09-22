using CustomerService.Application.Events;
using GenericMongo.Bases;

namespace RedisSub.Models;

public class Log : BaseEntity
{
    public string LogMessage { get; set; }
    public void FillLogMessage(CustomerCreated? createdEvent)
    {
        LogMessage = $"Customer Created: {createdEvent?.Id}, {createdEvent?.Address} ...";
    }
}

