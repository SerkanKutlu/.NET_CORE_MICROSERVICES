using CustomerService.Consumer.Models;

namespace CustomerService.Consumer.Interfaces;

public interface IConsumerService
{
    void LogMany(IEnumerable<Log> logs);
}