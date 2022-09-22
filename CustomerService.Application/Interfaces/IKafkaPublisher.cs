using Confluent.Kafka;
using CustomerService.Application.Events;

namespace CustomerService.Application.Interfaces;

public interface IKafkaPublisher
{
    Task Publish(CustomerCreated customerCreated);
    Task PublishToRetry(ConsumeResult<Null,CustomerCreated> message);
    Task PublishToError(ConsumeResult<Null,CustomerCreated> message);
}