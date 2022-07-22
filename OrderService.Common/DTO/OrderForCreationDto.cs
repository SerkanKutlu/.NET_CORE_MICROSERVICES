namespace OrderService.Common.DTO;

public class OrderForCreationDto
{
    public string CustomerId { get; set; }
    public string Status { get; set; }
    public List<string> ProductIds { get; set; }
}