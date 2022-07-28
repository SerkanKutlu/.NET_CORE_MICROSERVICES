namespace CustomerService.Consumer.Consumers;

public class ConsumerBase<T> where T:class
{
    protected readonly ILogger<T> Logger;

    public ConsumerBase(ILogger<T> logger)
    {
        Logger = logger;
    }
}