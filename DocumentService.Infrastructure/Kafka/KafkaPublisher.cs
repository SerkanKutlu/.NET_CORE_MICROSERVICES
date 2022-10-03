using Confluent.Kafka;
using Core.Entity;
using Core.Events;
using Core.Interfaces;

namespace DocumentService.Infrastructure.Kafka;

public class KafkaPublisher : IKafkaPublisher
{
    private readonly IKafkaSettings _kafkaSettings;
    private readonly IProducer<Null, DocumentUploaded> _producer;

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
        _producer  =  new ProducerBuilder<Null,DocumentUploaded>(producerConfig).SetValueSerializer(new DocumentUploaded()).Build();
    }
    public async Task Publish(DocumentUploaded documentUploaded)
    {
        Console.WriteLine("PUBLISHED REIS");
        Console.WriteLine(documentUploaded.DocumentId);
        var kafkaMessage = new Message<Null, DocumentUploaded>
        {
            Value = documentUploaded
        };
        await _producer.ProduceAsync(_kafkaSettings.DocumentUploadedTopic, kafkaMessage);
    }
    
}