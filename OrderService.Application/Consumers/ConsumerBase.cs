using Microsoft.Extensions.Logging;

namespace OrderService.Application.Consumers;

public class ConsumerBase<T> where T:class
{
    protected readonly ILogger<T> Logger;

    public ConsumerBase(ILogger<T> logger)
    {
        Logger = logger;
    }
}