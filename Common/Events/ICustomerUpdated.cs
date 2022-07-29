namespace Common.Events;

public interface ICustomerUpdated
{
    public string CustomerId { get; set; }
    public string LogMessage { get; set; }
    public DateTime UpdatedAt { get; set; }
}