namespace CustomerService.Application.Interfaces;

public interface IRabbitSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    
    public string Exchange { get; set; }
    public string RoutingKey { get; set; }
    
}