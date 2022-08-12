namespace GenericMongo.Bases;

public abstract class BaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CreatedAt { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
    public string UpdatedAt { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
}