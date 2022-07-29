namespace Common.Events;

public interface IOrderCreated
{
    public string OrderId { get; set; }
    public string LogMessage { get; set; }
    public DateTime CreatedAt { get; set; }
}