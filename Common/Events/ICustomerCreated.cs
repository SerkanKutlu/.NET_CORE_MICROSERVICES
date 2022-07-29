namespace Common.Events;

public interface ICustomerCreated
{
    public string CustomerId { get; set; }
    public string LogMessage { get; set; }
    public DateTime CreatedAt { get; set; }
}