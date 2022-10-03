using Core.Interfaces;

namespace DocumentService.Infrastructure.Kafka;

public class KafkaSettings : IKafkaSettings
{
    public string BootstrapServer { get; set; }
    public string DocumentUploadedTopic { get; set; }
    public string SASLProtocol { get; set; }
    public string SASLMechanism { get; set; }
    public string SASLUsername { get; set; }
    public string SASLPassword { get; set; }
}