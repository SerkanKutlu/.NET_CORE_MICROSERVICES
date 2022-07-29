namespace Common.Events;

public interface IOrderUpdated
{
    public string OrderId { get; set; }
    public string LogMessage { get; set; }
    public DateTime UpdatedAt { get; set; }
}