namespace CustomerService.Application.Interfaces;

public interface IKafkaSettings
{

    public string BootstrapServer { get; set; }
    public string CustomerCreatedTopic { get; set; }
    public string CustomerRetryTopic { get; set; }
    public string CustomerFailedTopic { get; set; }
    public string SASLProtocol { get; set; }
    public string SASLMechanism { get; set; }
    public string SASLUsername { get; set; }
    public string SASLPassword { get; set; }
    public string MainGroupId { get; set; }
    public string EnableAutoCommitForMain { get; set; }
    public string RetryGroupId { get; set; }
    public string EnableAutoCommitForRetry { get; set; }
    
}