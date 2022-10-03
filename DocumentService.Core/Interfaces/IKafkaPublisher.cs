using Confluent.Kafka;
using Core.Events;

namespace Core.Interfaces;

public interface IKafkaPublisher
{
    Task Publish(DocumentUploaded documentUploaded);
}