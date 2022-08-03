using OrderService.Domain.Entities;

namespace OrderService.Application.DTO;

public class ProductForUpdateDto
{
    public string Id { get; set; }
    public string ImageUrl { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    
    public Product ToProduct()
    {
        return new Product
        {
            ImageUrl = ImageUrl,
            Name = Name,
            Price = Price,
            Id = Id
        };
    }
}