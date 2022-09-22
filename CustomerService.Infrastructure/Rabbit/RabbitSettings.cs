using CustomerService.Application.Interfaces;

namespace CustomerService.Infrastructure.Rabbit;

public class RabbitSettings : IRabbitSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Exchange { get; set; }
    public string RoutingKey { get; set; }
    public string QueueName { get; set; }
    
}