namespace CustomerService.Application.Interfaces;

public interface IKafkaSettings
{

    public string BootstrapServer { get; set; }
    public string OrderCreatedTopic { get; set; }
    public string SASLProtocol { get; set; }
    public string SASLMechanism { get; set; }
    public string SASLUsername { get; set; }
    public string SASLPassword { get; set; }
    
}