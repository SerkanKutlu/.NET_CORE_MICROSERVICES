using Confluent.Kafka;
using CustomerService.Application.Events;
using CustomerService.Application.Interfaces;

namespace CustomerService.Infrastructure.Kafka;

public class KafkaPublisher : IKafkaPublisher
{
    private readonly IKafkaSettings _kafkaSettings;
    private readonly IProducer<Null, CustomerCreated> _producer;

    public KafkaPublisher(IKafkaSettings kafkaSettings)
    {
        _kafkaSettings = kafkaSettings;
        var options = new Dictionary<string, string>
        {
            {"bootstrap.servers", _kafkaSettings.BootstrapServer},
            {"security.protocol", _kafkaSettings.SASLProtocol},
            {"sasl.mechanisms", _kafkaSettings.SASLMechanism},
            {"sasl.username", _kafkaSettings.SASLUsername},
            {"sasl.password", _kafkaSettings.SASLPassword}
        };
        var clientConfig = new ClientConfig(options);
        var producerConfig = new ProducerConfig(clientConfig);
        _producer  =  new ProducerBuilder<Null,CustomerCreated>(producerConfig).SetValueSerializer(new CustomerCreated()).Build();
    }
    public async Task Publish(CustomerCreated customerCreated)
    {
        var kafkaMessage = new Message<Null, CustomerCreated>
        {
            Value = customerCreated
        };
        await _producer.ProduceAsync(_kafkaSettings.OrderCreatedTopic, kafkaMessage);
    }
}