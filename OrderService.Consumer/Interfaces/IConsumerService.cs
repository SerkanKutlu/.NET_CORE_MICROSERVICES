using OrderService.Consumer.Models;

namespace OrderService.Consumer.Interfaces;

public interface IConsumerService
{
    void LogMany(IEnumerable<Log> logs);
}